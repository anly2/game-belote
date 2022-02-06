using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Belote.Domain;
using Belote.Game.State;
using static Belote.Domain.ContractUtils;
using static Belote.Player.Ai.GameUtils;

namespace Belote.Player.Ai.Basic
{
    public class BasicAiPlayer : NamedPlayer
    {
        private static readonly IReadOnlyList<IReadOnlyList<Regex>> BidPatterns = new[]
        {
            "J 9",
            "J, A, A"
        }.Select(pattern => pattern.ToCardsPattern().ToList().AsReadOnly()).ToList().AsReadOnly();


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
                var found = _state!.CurrentHand.FindMatchingCards(pattern);
                if (found.Any())
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