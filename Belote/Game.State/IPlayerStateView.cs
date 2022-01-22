using System;
using System.Collections.Generic;
using Belote.Domain;

namespace Belote.Game.State
{
    public interface IPlayerStateView
    {
        public int PlayerIndex { get; }
        public IReadOnlyList<Card> CurrentHand { get; }
        public IReadOnlyList<Card> CurrentTrick { get; }
        public int? CurrentTrickInitiator { get; }
        public int CurrentMatchDealer { get; }
        public Contract? CurrentContract { get; }
        public bool CommittedToCurrentContract { get; }


        event Action<int, Contract> OnBid;

        //event OnPlayedCard
        //event OnDeclaration
        //event OnTrickEnd
        //event OnMatchEnd
    }
}