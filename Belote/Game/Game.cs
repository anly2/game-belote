using System;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;
using Belote.Players;
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
            deck.RemoveAll(card => card.Rank() < 6);
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
            // init match state
            var prevDealer = _match.Dealer;

            _match = _match.Fresh();

            _match.Dealer = _state.NextPlayer(prevDealer);


            // play match
            DealInitial();
            if (!GatherBids()) return;
            DealRemaining();

            //PLAYERS

            // finish match
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
                _state.Deck.Move(3, _match.PlayerCards[i]);
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

                var bid = player.Bid();

                if (bid == null)
                {
                    passes++;
                    continue;
                }

                if (bid <= currentBid)
                    throw new InvalidOperationException(
                        $"Player ${playerIndex} made an invalid bid of ${bid} when ${currentBid} has already been called!");

                currentBid = bid;
                _match.Contract = currentBid;
                _match.CommittedPlayer = playerIndex;
            }

            return currentBid != null;
        }
    }
}