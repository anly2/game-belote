using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Belote.Domain;
using Belote.Game;
using Belote.Game.State;
using static Belote.Domain.ContractUtils;

namespace Belote.Player.Human.Console
{
    public class ConsoleHumanPlayer : NamedPlayer
    {
        private IPlayerStateView? _state;
        private readonly bool _printEvents;

        public ConsoleHumanPlayer(string? name = null, bool printEvents = false)
        {
            Name = name;
            _printEvents = printEvents;
        }

        public override void BindStateView(IPlayerStateView playerStateView)
        {
            base.BindStateView(playerStateView);

            _state = playerStateView;

            if (_printEvents)
            {
                _state.OnBid += (i, contract) => System.Console.Out.WriteLine($"Player {i+1} bid: {contract}");
                _state.OnTrickEnd += t => System.Console.Out.WriteLine($"Player {t.winner+1} won a trick: {t.trickCards.Text()}");
            }
        }


        public override Contract? Bid()
        {
            Print("Your turn to bid:");
            Print("Cards: " + _state!.CurrentHand.Text());

            PrintBidOptions();
            if (!int.TryParse(System.Console.ReadLine(), out var bid)) bid = 0;
            bid--;

            if (bid == 6)
            {
                return _state.CurrentContract?.IsContre() == true ?
                    _state.CurrentContract?.Recontra() : _state.CurrentContract?.Contre();
            }

            if (bid < 0) return null;
            return PlainContracts[bid];
        }

        private void PrintBidOptions()
        {
            var currentContract = (int?) _state?.CurrentContract ?? -1;

            var options = new[] { ("pass", true) }.Concat(
                PlainContracts.Select(contract => (contract.Text(), (int) contract > currentContract)) ).ToList();

            if (currentContract >= 0 && !_state!.IsCommittedToCurrentContract)
            {
                var c = (Contract) currentContract;
                options.Add( ((c.IsPlain() ? c.Contre() : c.Recontra()).Text(), true) );
            }

            PrintOptions(options.ToArray());
        }

        public override Card Play(List<Declaration> declarations)
        {
            var contract = _state!.CurrentContract!.Value;
            var hand = _state!.CurrentHand;
            var trick = _state!.CurrentTrick;

            Print($"Your turn to play : ({contract})  {trick.Text()}");
            Print("Cards: " + hand.Text());

            var playableCards = hand.PlayableCards(contract, trick).ToList();
            PrintOptions(1, playableCards.Select(c => (c.Text(), true)).ToArray());

            if (playableCards.Count == 1)
            {
                System.Console.ReadLine(); //anything
                return playableCards[0];
            }

            if (!int.TryParse(System.Console.ReadLine(), out var choice))
                throw new ArgumentException("Invalid choice!");
            choice--;

            return playableCards[choice];
        }



        private static void PrintOptions(params (string text, bool available)[] options)
        {
            PrintOptions(0, options);
        }
        private static void PrintOptions(int start, params (string text, bool available)[] options)
        {
            var sb = new StringBuilder();
            sb.Append('(');

            var n = start;
            foreach (var (text, available) in options)
            {
                sb.Append(' ').Append(available ? n.ToString() : "-").Append(':').Append(text);
                n++;
            }

            sb.Append(" )");
            System.Console.Out.WriteLine(sb.ToString());
        }
    }
}