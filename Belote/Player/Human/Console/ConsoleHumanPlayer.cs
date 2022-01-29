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
    public class ConsoleHumanPlayer : IPlayer
    {
        private IPlayerStateView? _state;
        // ReSharper disable once InconsistentNaming
        private readonly string? Name;

        public ConsoleHumanPlayer(string? name)
        {
            Name = name;
        }

        public void BindStateView(IPlayerStateView playerStateView)
        {
            _state = playerStateView;
        }

        protected void Print(string message)
        {
            System.Console.Out.WriteLine("[Player " + (_state?.PlayerIndex + 1) + (Name??"")+ "] " + message);
        }


        public Contract? Bid()
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

        public Card Play(List<Declaration> declarations)
        {
            var contract = _state!.CurrentContract!.Value;
            var hand = _state!.CurrentHand;
            var trick = _state!.CurrentTrick;

            Print($"Your turn to play : ({contract})  {trick.Text()}");
            Print("Cards: " + hand.Text());

            PrintOptions(1, hand.Select(c => (c.Text(), c.CanBePlayed(contract, hand, trick))).ToArray());

            if (!int.TryParse(System.Console.ReadLine(), out var choice))
                throw new ArgumentException("Invalid choice!");
            choice--;

            return hand[choice];
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

            sb.Append(')');
            System.Console.Out.WriteLine(sb.ToString());
        }
    }
}