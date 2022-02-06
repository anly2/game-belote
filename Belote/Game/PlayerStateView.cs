using System;
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
                GameState = gameState;
                PlayerIndex = playerIndex;
                CurrentHand = new ReadOnlyCollection<Card>(gameState.Match.PlayerCards[playerIndex]);
                CurrentTrick = new ReadOnlyCollection<Card>(gameState.Match.TrickCards);
            }

            public GameState GameState { get; }
            IGameState IPlayerStateView.GameState => GameState;

            public int PlayerIndex { get; }
            public IReadOnlyList<Card> CurrentHand { get; }
            public IReadOnlyList<Card> CurrentTrick { get; }

            int? IPlayerStateView.CurrentTrickInitiator => GameState.Match.TrickInitiator;

            int IPlayerStateView.CurrentTrickCount => GameState.Match.TrickCount;

            int IPlayerStateView.CurrentMatchDealer => GameState.Match.Dealer;

            Contract? IPlayerStateView.CurrentContract => GameState.Match.Contract;

            bool IPlayerStateView.IsCommittedToCurrentContract => GameState.Match.CommittedPlayer != null &&
                    GameState.PlayerTeams[PlayerIndex] == GameState.PlayerTeams[GameState.Match.CommittedPlayer.Value];


            // Delegate events //

            event Action<int, Contract>? IPlayerStateView.OnBid
            {
                add => GameState.Match.OnBid += value;
                remove => GameState.Match.OnBid -= value;
            }

            event Action<(IEnumerable<Card> trickCards, int initiator, int winner)>? IPlayerStateView.OnTrickEnd
            {
                add => GameState.Match.OnTrickEnd += value;
                remove => GameState.Match.OnTrickEnd -= value;
            }
        }
    }
}