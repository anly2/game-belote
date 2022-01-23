using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Player.Human.Console
{
    public class ConsoleHumanPlayer : IPlayer
    {
        private IPlayerStateView? _state;
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
            Print("Cards: " + _state?.CurrentHand.Text());

            PrintBidOptions();
            var bid = Convert.ToInt32(System.Console.ReadLine()) - 1;
            if (bid < 0) return null;
            return ContractUtils.PlainContracts[bid];
        }

        private void PrintBidOptions()
        {
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append("0:pass");

            var currentContract = (int?) _state?.CurrentContract ?? -1;

            var n = 0;
            foreach (var contract in ContractUtils.PlainContracts)
            {
                ++n;
                sb.Append(' ').Append((int) contract <= currentContract ?  "-" : n.ToString()).Append(':').Append(contract.Text());
            }

            //FIXME do this logic properly
            if (currentContract >= 0)
            {
                sb.Append(' ').Append(++n).Append(':').Append(((Contract) currentContract).Contre().Text());
                sb.Append(' ').Append(++n).Append(':').Append(((Contract) currentContract).Recontra().Text());
            }

            sb.Append(')');
            System.Console.Out.WriteLine(sb.ToString());
        }

        public Card Play(List<Declaration> declarations)
        {
            return _state.CurrentHand[0];
        }
    }
}