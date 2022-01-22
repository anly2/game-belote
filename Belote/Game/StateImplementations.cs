using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;
using Belote.Players;

namespace Belote.Game
{

    public partial class Game {

        // State implementations //

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

            public Contract? Contract { get; set; }

            public int? CommittedPlayer { get; set; }

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
}