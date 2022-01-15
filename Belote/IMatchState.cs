using System.Collections.Generic;

namespace Belote
{
    public interface IMatchState
    {
        public byte Dealer { get; }
        public IReadOnlyList<IReadOnlyList<Card>> PlayerCards { get; }
        public byte? CommittedPlayer { get; }
        public IReadOnlyList<Declaration> Declarations { get; }
        public byte? TrickInitiator { get; }
        public IReadOnlyList<Card> TrickCards { get; }
        public IReadOnlyList<IReadOnlyList<Card>> WonCards { get; }
    }
}