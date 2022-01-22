using System.Collections.Generic;

namespace Belote
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
        
        //event OnPlayedCard
        //event OnDeclaration
        //event OnTrickEnd
        //event OnMatchEnd
    }
}