using System.Runtime.CompilerServices;
using Belote.Game.State;

namespace Belote.Game
{
    public static class IndexUtils
    {
        public static int NextPlayer(this IGameState gameState, int? playerIndex)
        {
            if (playerIndex == null)
                return 0;

            return (playerIndex.Value + 1) % gameState.Players.Count;
        }

        public static bool AreTeamMates(this IGameState gameState, int playerA, int playerB)
        {
            return gameState.PlayerTeams[playerA] == gameState.PlayerTeams[playerB];
        }

        public static bool IsTeamMate(this IPlayerStateView playerStateView, int otherPlayerIndex)
        {
            return playerStateView.GameState.AreTeamMates(playerStateView.PlayerIndex, otherPlayerIndex);
        }
    }
}