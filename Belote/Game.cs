using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommonUtils;

namespace Belote
{
    public class Game
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
            return Enumerable.Range(0, players.Count-1).Select(b => (byte) b).ToList().AsReadOnly();
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
            // GatherBids();
            // DealRemaining();

            // finish match
            // var scores = CountScore();
            // for (var i = 0; i < State.Scores.Length; i++)
            //     State.Scores[i] += scores[i];
        }

        protected virtual void DealInitial()
        {
            if (_match == null) throw new InvalidOperationException("There is no Match started!");
            
            for (var i = 0; i < _state.Players.Count; i++)
                _state.Deck.Move(3, _match.PlayerCards[i]);
            
            for (var i = 0; i < _state.Players.Count; i++)
                _state.Deck.Move(2, _match.PlayerCards[i]);
        }
        
        
        private class GameState : IGameState
        {
            public GameState(List<Card> deck, List<IPlayer> players)
            {
                Deck = deck;
                Players = players;
                PlayerTeams = AssignPlayerTeams(players);
                Scores = new List<byte>(new byte[PlayerTeams.Distinct().Count()]);
                Match = new MatchState(players.Count);
            }

            public readonly List<Card> Deck;
            IReadOnlyList<Card> IGameState.Deck => Deck.AsReadOnly();

            public IReadOnlyList<IPlayer> Players { get; }

            public IReadOnlyList<byte> PlayerTeams { get; }
            
            public readonly List<byte> Scores;
            IReadOnlyList<byte> IGameState.Scores => Scores.AsReadOnly();

            public MatchState Match { get; set; }
            IMatchState IGameState.Match => Match;
        }

        private class MatchState : IMatchState
        {
            public MatchState(int playerCount)
            {
                PlayerCards = new List<IList<Card>>();
                Contract = null; 
                Declarations = new List<Declaration>();
                TrickCards = new List<Card>();
                WonCards = new List<IList<Card>>();
                
                for (int i = 0; i < playerCount; i++)
                {
                    PlayerCards.Add(new List<Card>(8));
                    WonCards.Add(new List<Card>());
                }
            }

            public MatchState Fresh()
            {
                foreach (var hand in PlayerCards) hand.Clear();
                foreach (var pile in WonCards) pile.Clear();
                Declarations.Clear();
                TrickCards.Clear();
                Dealer = 0;
                Contract = null;
                CommittedPlayer = null;
                TrickInitiator = null;
                return this;
            }


            public int Dealer { get; set; }
            
            public IList<IList<Card>> PlayerCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.PlayerCards => PlayerCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();
            
            public Contract? Contract { get; private set; }
            
            public int? CommittedPlayer { get; private set; }

            public IList<Declaration> Declarations { get; }
            IReadOnlyList<Declaration> IMatchState.Declarations => new ReadOnlyCollection<Declaration>(Declarations); 
            
            public int? TrickInitiator { get; private set; }
            
            public IList<Card> TrickCards { get; }
            IReadOnlyList<Card> IMatchState.TrickCards => new ReadOnlyCollection<Card>(TrickCards); 
            
            public IList<IList<Card>> WonCards { get; }
            IReadOnlyList<IReadOnlyList<Card>> IMatchState.WonCards => WonCards.Select(s => new ReadOnlyCollection<Card>(s)).ToList().AsReadOnly();
        }

        private class PlayerStateView : IPlayerStateView
        {
            public PlayerStateView(GameState gameState, int playerIndex)
            {
                _gameState = gameState;
                PlayerIndex = playerIndex;
                CurrentHand = new ReadOnlyCollection<Card>(gameState.Match.PlayerCards[playerIndex]);
                CurrentTrick = new ReadOnlyCollection<Card>(gameState.Match.TrickCards);
            }

            private readonly GameState _gameState;

            public int PlayerIndex { get; }
            public IReadOnlyList<Card> CurrentHand { get; }
            public IReadOnlyList<Card> CurrentTrick { get; }

            int? IPlayerStateView.CurrentTrickInitiator => _gameState.Match.TrickInitiator;

            int IPlayerStateView.CurrentMatchDealer => _gameState.Match.Dealer;

            Contract? IPlayerStateView.CurrentContract => _gameState.Match.Contract;

            bool IPlayerStateView.CommittedToCurrentContract => _gameState.Match.CommittedPlayer != null &&
                    _gameState.PlayerTeams[PlayerIndex] == _gameState.PlayerTeams[_gameState.Match.CommittedPlayer.Value];
        }
    }

    public static class IndexUtils
    {
        public static int NextPlayer(this IGameState gameState, int? playerIndex)
        {
            if (playerIndex == null)
                return 0;

            return (playerIndex.Value + 1) % gameState.Players.Count;
        }
    }
    
    public static class BeloteCardSemantics
    {
        private static readonly int[] CardPowers =
        {
            // 2,  3,  4,  5,  6, 7, 8, 9, 10, J, Q, K, A
              -1, -1, -1, -1, -1, 0, 1, 2,  6, 3, 4, 5, 7
        };
        
        private static readonly int[] CardTrumpPowers =
        {
            // 2,  3,  4,  5,  6, 7, 8, 9, 10, J, Q, K, A
              -1, -1, -1, -1, -1, 0, 1, 8,  6, 9, 4, 5, 7
        };
        
        private static readonly int[] CardValues =
        {
            //7, 8, 9,  J,  Q, K, 10, A,  T9, TJ 
              0, 0, 0,  2,  3, 4, 10, 11, 14, 20
        };
        
        public static int Power(this Card card) => CardPowers[card.Rank()];
        public static int PowerWhenTrump(this Card card) => CardTrumpPowers[card.Rank()];
        
        public static int Value(this Card card) => CardValues[card.Power()];
        public static int ValueWhenTrump(this Card card) => CardValues[card.PowerWhenTrump()];
    }
}