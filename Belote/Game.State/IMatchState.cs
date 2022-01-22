using System;
using System.Collections.Generic;
using Belote.Domain;

namespace Belote.Game.State
{
    public interface IMatchState
    {
        public int Dealer { get; }
        public IReadOnlyList<IReadOnlyList<Card>> PlayerCards { get; }
        public Contract? Contract { get; }
        public int? CommittedPlayer { get; }
        public IReadOnlyList<Declaration> Declarations { get; }
        public int? TrickInitiator { get; }
        public IReadOnlyList<Card> TrickCards { get; }
        public IReadOnlyList<IReadOnlyList<Card>> WonCards { get; }


        public event Action<int, Contract> OnBid;
    }
}