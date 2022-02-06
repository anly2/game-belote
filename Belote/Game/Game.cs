using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;
using Belote.Player;
using CommonUtils;
using static System.String;

namespace Belote.Game
{
    public partial class Game
    {
        public const byte WinningScore = 151;


        private readonly GameState _state;
        public IGameState State => _state;

        // ReSharper disable once InconsistentNaming
        private MatchState _match
        {
            get => _state.Match;
            set => _state.Match = value;
        }


        public Game(List<Card> deck, List<IPlayer> players)
        {
            _state = new GameState(deck, players);
            BindPlayerStateViews();
        }

        private void BindPlayerStateViews()
        {
            for (var i = 0; i < _state.Players.Count; i++)
            {
                _state.Players[i].BindStateView(new PlayerStateView(_state, i));
            }
        }


        public static List<Card> GetPlayingDeck()
        {
            var deck = new List<Card>(Enum.GetValues<Card>());
            deck.RemoveAll(card => card.Rank() < 5);
            return deck;
        }


        public static IReadOnlyList<byte> AssignPlayerTeams(IList<IPlayer> players)
        {
            if (players.Count == 4)
                return new byte[] {0, 1, 0, 1};
            return Enumerable.Range(0, players.Count - 1).Select(b => (byte) b).ToList().AsReadOnly();
        }


        public virtual void PlayGame()
        {
            ShuffleBeforeGame();
            //#! With this naive condition the rule of "Cannot win with Valat" is not implemented
            while (!State.Scores.Any(s => s >= WinningScore))
            {
                PlayMatch();
                //TODO: use events
                Console.Out.WriteLine("Match ended. Score: " + Join(", ", _state.Scores));
            }
        }

        protected virtual void ShuffleBeforeGame()
        {
            _state.Deck.Shuffle();
        }

        protected virtual void PlayMatch()
        {
            // Init match state
            var prevDealer = _match.Dealer;

            _match = _match.Fresh();

            _match.Dealer = _state.NextPlayer(prevDealer);
            CutDeckBeforeMatch(prevDealer);


            // Match bidding phase
            DealInitial();
            if (!GatherBids())
            {
                MergeDeckBack(_match.PlayerCards);
                return;
            }
            DealRemaining();

            // Match trick playing phase
            _match.TrickInitiator = _state.NextPlayer(_match.Dealer);
            // Assume all players have the same number of cards
            while (_match.PlayerCards[0].Count > 0)
            {
                PlayTrick();
                _match.TrickCount++;
            }

            // Match scoring phase
            DoScoring();
            // var scores = CountScore();
            // for (var i = 0; i < State.Scores.Length; i++)
            //     State.Scores[i] += scores[i];

            MergeDeckBack(_match.WonCards);
        }

        protected virtual void DealInitial()
        {
            for (var i = 0; i < _state.Players.Count; i++)
                _state.Deck.Move(3, _match.PlayerCards[i]);

            for (var i = 0; i < _state.Players.Count; i++)
                _state.Deck.Move(2, _match.PlayerCards[i]);
        }

        protected virtual void DealRemaining()
        {
            for (var i = 0; i < _state.Players.Count; i++)
            {
                _state.Deck.Move(3, _match.PlayerCards[i]);
                _match.PlayerCards[i].SortHand(_match.Contract);
            }
        }

        protected virtual bool GatherBids()
        {
            Contract? currentBid = null;
            var playerIndex = _match.Dealer;
            var passes = 0;
            while (passes < 4)
            {
                playerIndex = _state.NextPlayer(playerIndex);
                var player = _state.Players[playerIndex];

                Contract? bid = null;
                if (_state.Match.CommittedPlayer != playerIndex)
                {
                    _match.PlayerCards[playerIndex].SortHand(currentBid);
                    bid = player.Bid();
                }


                if (bid == null)
                {
                    passes++;
                    _match.Bid(playerIndex, bid);
                    continue;
                }
                passes = 0;


                if (bid <= currentBid)
                    throw new InvalidOperationException(
                        $"Player {playerIndex} made an invalid bid of {bid} when {currentBid} has already been called!");
                if ((bid.Value.IsContre() || bid.Value.IsRecontra())
                    && _match.CommittedPlayer.HasValue
                    && _state.AreTeamMates(playerIndex, _match.CommittedPlayer.Value))
                    throw new InvalidOperationException(
                        $"Player {playerIndex} cannot double the bid of their teammate (player {_match.CommittedPlayer})!");

                currentBid = bid;
                _match.Bid(playerIndex, bid);
            }

            return currentBid != null;
        }

        protected virtual void PlayTrick()
        {
            var playerIndex = _match.TrickInitiator!.Value;
            var playersCount = _state.Players.Count;
            for (var i = 0; i < playersCount; i++)
            {
                if (i > 0) playerIndex = _state.NextPlayer(playerIndex);
                var player = _state.Players[playerIndex];

                var played = player.Play(null);

                Debug.Assert(
                    _match.PlayerCards[playerIndex].PlayableCards(_match.Contract!.Value, _match.TrickCards).Contains(played),
                    $"Player '{player}' tried to play card {played.Text()} which is not allowed at this time!"
                );

                _match.PlayerCards[playerIndex].Move(played, _match.TrickCards);
                _match.PlayCard(playerIndex, played);
            }

            var winner = DecideTrickWinner();
            _match.EndTrick(_match.TrickCards, _match.TrickInitiator!.Value, winner);
            _match.TrickCards.Move(playersCount, _match.WonCards[winner]);
            _match.TrickInitiator = winner;
        }

        protected virtual int DecideTrickWinner()
        {
            var trick = _match.TrickCards;
            var offset = trick.IndexOf(trick.StrongestCard(_match.Contract));
            return _state.NextPlayer(_match.TrickInitiator, offset);
        }

        protected virtual void DoScoring()
        {
            var scores = new int[_state.Scores.Count];

            //Count the card scores
            for (var i = 0; i < _match.WonCards.Count; i++)
            {
                var score = CountScore(_match.WonCards[i], _match.Contract!.Value);
                scores[_state.PlayerTeams[i]] += score;
            }

            //Add the 10 points for last trick won
            var lastTrickWinner = _match.TrickInitiator!.Value;
            scores[_state.PlayerTeams[lastTrickWinner]] += 10;

            //Include declarations
            //TODO

            //If committed team failed to fulfil the contract, spread there points to the others
            var committedTeam = _state.PlayerTeams[_match.CommittedPlayer!.Value];
            var scoreOfCommittedTeam = scores[committedTeam];
            if (scoreOfCommittedTeam != scores.Max())
            {
                for (var i = 0; i < scores.Length; i++)
                    scores[i] += scoreOfCommittedTeam / (scores.Length - 1);
                scores[committedTeam] = 0;
            }

            //Convert match points to game points
            for (var i = 0; i < scores.Length; i++)
                _state.Scores[i] += (byte) MatchScoreToGameScore(scores[i], _match.Contract!.Value);
        }

        public int CountScore(IEnumerable<Card> pile, Contract contract) //TODO: include Declarations
        {
            return pile.Sum(c => c.Value(contract));
        }

        public int MatchScoreToGameScore(int matchScore, Contract contract)
        {
            //TODO: implement special rounding
            return (int) Math.Round((double) matchScore / 10);
        }



        protected virtual void MergeDeckBack(IList<List<Card>> piles)
        {
            //TODO: implement as a Strategy
            piles = new List<List<Card>>(piles);
            piles.Shuffle();
            foreach (var pile in piles)
            {
                _state.Deck.AddRange(pile);
                pile.Clear();
            }
        }

        protected virtual void CutDeckBeforeMatch(int cuttingPlayer)
        {
            // _state.Players[cuttingPlayer].CutDeck();
        }
    }
}