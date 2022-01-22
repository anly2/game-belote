using System.Collections.Generic;
using System.Collections.ObjectModel;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Game
{

    public partial class Game {

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