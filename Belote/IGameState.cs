using System.Collections.Generic;

namespace Belote
{
    public interface IGameState
    {
        public IReadOnlyList<Card> Deck { get; }
        public IReadOnlyList<byte> Scores { get; }
        public IReadOnlyList<IPlayer> Players { get; }
        public IReadOnlyList<byte> PlayerTeams { get; }
    }
}