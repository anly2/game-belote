using System;
using System.Collections.Generic;
using Belote.Domain;

namespace Belote.Game.State
{
    public interface IPlayerStateView
    {
        public IGameState GameState { get; }
        public int PlayerIndex { get; }
        public IReadOnlyList<Card> CurrentHand { get; }
        public IReadOnlyList<Card> CurrentTrick { get; }
        public int? CurrentTrickInitiator { get; }
        public int CurrentMatchDealer { get; }
        public Contract? CurrentContract { get; }
        public bool IsCommittedToCurrentContract { get; }


        event Action<int, Contract> OnBid;
        event Action<(IEnumerable<Card> trickCards, int initiator, int winner)>? OnTrickEnd;

        //event OnPlayedCard
        //event OnDeclaration
        //event OnTrickEnd
        //event OnMatchEnd
    }
}