using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private static readonly ReadOnlyCollection<(string source, ReadOnlyCollection<Regex> pattern, Contract? contract)> BidPatterns = new(string pattern, Contract? contract)[]
        {
            ("J 9", null),
            ("J, A, A", null),
            ("A, A, A", Contract.NoTrumps),
            ("A 10 K Q", Contract.NoTrumps),
            ("J, J, J", Contract.AllTrumps),
            ("J 9, J 9", Contract.AllTrumps)
        }.Select(e => (e.pattern, e.pattern.ToCardsPattern().ToList().AsReadOnly(), e.contract)).ToList().AsReadOnly();


        private IPlayerStateView? _state;

        public override void BindStateView(IPlayerStateView playerStateView)
        {
            base.BindStateView(playerStateView);
            _state = playerStateView;
        }

        public override Contract? Bid()
        {
            Print(_state!.CurrentHand.Text());
            foreach (var (source, pattern, contract) in BidPatterns)
            {
                var found = _state!.CurrentHand.FindMatchingCards(pattern);
                if (found?.Any() ?? false)
                {
                    Console.Out.WriteLine("Recognized pattern: " + source);
                    return contract ?? PlainContracts[found[0].Suit()];
                }
            }
            Print("passing");
            return null;
        }

        public override Card Play(List<Declaration> declarations)
        {
            throw new NotImplementedException();
        }
    }
}