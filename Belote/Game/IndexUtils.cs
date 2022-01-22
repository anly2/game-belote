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
    }
}