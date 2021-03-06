using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Belote.Domain;
using Belote.Game;
using Belote.Game.State;
using CommonUtils;
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

        protected override void Print(string message) => Debug.WriteLine("[" + Name + "] " + message);

        public override Contract? Bid()
        {
            Print(_state!.CurrentHand.Text());
            foreach (var (source, pattern, contract) in BidPatterns)
            {
                var found = _state!.CurrentHand.FindMatchingCards(pattern);
                if (found?.Any() ?? false)
                {
                    Print("recognized pattern: " + source);
                    var bid = contract ?? PlainContracts[found[0].Suit()];
                    if (!_state.CurrentContract.HasValue || bid > _state.CurrentContract.Value)
                        return bid;
                    Print($"Not bidding {bid} as already on {_state.CurrentContract}");
                }
            }
            Print("passing");
            return null;
        }

        public override Card Play(List<Declaration> declarations)
        {
            var played = DoPlay(declarations);
            Print(_state!.CurrentHand.Text());
            Print("playing: " + played.Text());
            return played;
        }

        private Card DoPlay(List<Declaration> declarations)
        {
            if (IsTrickInitiator())
            {
                var cards = new List<Card>(_state!.CurrentHand);
                cards.Sort((a, b) => a.CompareTo(b, _state.CurrentContract));

                return _state.CurrentTrickCount < 4 ? cards.First() : cards.Last();
            }

            var playableCards = new List<Card>(_state!.CurrentHand.PlayableCards(_state.CurrentContract!.Value, _state.CurrentTrick));
            playableCards.Sort((a, b) => a.CompareTo(b, _state.CurrentContract));
            return playableCards.First();
        }

        private bool IsTrickInitiator()
        {
            return _state!.CurrentTrickInitiator == _state!.PlayerIndex;
        }
    }
}