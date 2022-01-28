using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;
using Belote.Player;
using CommonUtils;

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
            //#! With this naive condition the rule of "Cannot win with Valat" is not implemented
            while (!State.Scores.Any(s => s >= WinningScore))
            {
                ShuffleBeforeGame();
                PlayMatch();
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


            // Match bidding phase
            DealInitial();
            if (!GatherBids()) return;
            DealRemaining();

            // Match trick playing phase
            _match.TrickInitiator = _state.NextPlayer(_match.Dealer);
            // Assume all players have the same number of cards
            while (_match.PlayerCards[0].Count > 0)
            {
                PlayTrick();
            }

            // Match scoring phase
            // var scores = CountScore();
            // for (var i = 0; i < State.Scores.Length; i++)
            //     State.Scores[i] += scores[i];
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
                    continue;
                }

                if (bid <= currentBid)
                    throw new InvalidOperationException(
                        $"Player {playerIndex} made an invalid bid of {bid} when {currentBid} has already been called!");
                if ((bid.Value.IsContre() || bid.Value.IsRecontra())
                    && _match.CommittedPlayer.HasValue
                    && _state.AreTeamMates(playerIndex, _match.CommittedPlayer.Value))
                    throw new InvalidOperationException(
                        $"Player {playerIndex} cannot double the bid of their teammate (player {_match.CommittedPlayer})!");

                currentBid = bid;
                _match.Bid(playerIndex, bid.Value);
            }

            return currentBid != null;
        }

        protected virtual void PlayTrick()
        {
            var playerIndex = _match.TrickInitiator!.Value;
            var playersCount = _state.Players.Count;
            for (var i = 0; i < playersCount; i++)
            {
                playerIndex = _state.NextPlayer(playerIndex);
                var player = _state.Players[playerIndex];

                var played = player.Play(null);

                Debug.Assert(_match.PlayerCards[playerIndex].Contains(played),
                    $"Player {playerIndex} tried to play card {played.Text()} which is not in their hand!");
                //Preferably check other rules, but in a real human game there is no Game Master that enforces those..

                _match.PlayerCards[playerIndex].Move(played, _match.TrickCards);
            }

            var winner = DecideTrickWinner();
            _match.TrickCards.Move(playersCount, _match.WonCards[winner]);
            _match.TrickInitiator = winner;
        }

        protected virtual int DecideTrickWinner()
        {
            var strongestCard = _match.TrickCards[0];
            var strongestPlayer = _match.TrickInitiator!.Value;

            var contract = _match.Contract!.Value;
            var playerIndex = strongestPlayer;
            foreach (var card in _match.TrickCards)
            {
                playerIndex = _state.NextPlayer(playerIndex);
                if (strongestCard.IsTrump(contract) || !card.IsTrump(contract)) continue;
                if (strongestCard.CompareTo(card, contract) >= 0) continue;
                strongestCard = card;
                strongestPlayer = playerIndex;
            }

            return strongestPlayer;
        }
    }
}