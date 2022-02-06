using System;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;
using Belote.Game.State;
using static Belote.Domain.ContractUtils;
using static Belote.Player.Ai.GameUtils;

namespace Belote.Player.Ai.Basic
{
    public class BasicAiPlayer : NamedPlayer
    {
        private static readonly IReadOnlyList<IReadOnlyList<CardSelector>> BidPatterns = new[]
        {
            new [] {Card.Spades_J, Card.Spades_9},
            new [] {Card.Spades_J, Card.Diamonds_A, Card.Hearts_A}
        }.Select(cards => cards.Select(c => new CardSelector(c)).ToList().AsReadOnly()).ToList().AsReadOnly();


        private IPlayerStateView? _state;

        public override void BindStateView(IPlayerStateView playerStateView)
        {
            base.BindStateView(playerStateView);
            _state = playerStateView;
        }

        public override Contract? Bid()
        {
            foreach (var pattern in BidPatterns)
            {
                var found = _state!.CurrentHand.MatchRanks(pattern);
                if (found != null)
                    return PlainContracts[found[0].Suit()];
            }
            return null;
        }

        public override Card Play(List<Declaration> declarations)
        {
            throw new NotImplementedException();
        }
    }
}