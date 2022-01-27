using System;
using System.Collections.Generic;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Player.Ai.Basic
{
    public class BasicAiPlayer : IPlayer
    {
        private static readonly IReadOnlyList<IReadOnlyList<Card>> BidPatterns = new[]
        {
            new [] {Card.Spades_J, Card.Spades_9},
            new [] {Card.Spades_J, Card.Diamonds_A, Card.Hearts_A}
        };


        // ReSharper disable once InconsistentNaming
        private IPlayerStateView? State;

        public void BindStateView(IPlayerStateView playerStateView)
        {
            State = playerStateView;
        }
        #pragma warning disable CS8602

        public Contract? Bid()
        {
            foreach (var pattern in BidPatterns)
            {
                var found = State.CurrentHand.MatchRanks(pattern);
                if (found != null)
                    return ContractUtils.PlainContracts[found[0].Suit()];
            }
            return null;
        }

        public Card Play(List<Declaration> declarations)
        {
            throw new NotImplementedException();
        }

        #pragma warning restore CS8602
    }
}