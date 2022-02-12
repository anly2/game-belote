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
        public int TrickCount { get; }
        public IReadOnlyList<Card> TrickCards { get; }
        public IReadOnlyList<IReadOnlyList<Card>> WonCards { get; }


        public event Action<(int player, Contract? bid)> OnBid;
        public event Action<IMatchState> OnBiddingEnd;
        public event Action<(int player, Card card)>? OnCardPlayed;
        public event Action<(IEnumerable<Card> trickCards, int initiator, int winner)>? OnTrickEnd;
    }
}