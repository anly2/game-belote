using System.Collections.Generic;
using System.Text;
using Belote.Domain;
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
            var sb = new StringBuilder();
            sb.Append('(');
            sb.Append("0:pass");

            var currentContract = (int?) _state?.CurrentContract ?? -1;

            var n = 0;
            foreach (var contract in PlainContracts)
            {
                ++n;
                sb.Append(' ').Append((int) contract <= currentContract ?  "-" : n.ToString()).Append(':').Append(contract.Text());
            }

            if (currentContract >= 0 && !_state!.IsCommittedToCurrentContract)
            {
                if (((Contract) currentContract).IsPlain())
                    sb.Append(' ').Append(++n).Append(':').Append(((Contract) currentContract).Contre().Text());
                if (((Contract) currentContract).IsContre())
                    sb.Append(' ').Append(++n).Append(':').Append(((Contract) currentContract).Recontra().Text());
            }

            sb.Append(')');
            System.Console.Out.WriteLine(sb.ToString());
        }

        public Card Play(List<Declaration> declarations)
        {
            return _state!.CurrentHand[0];
        }
    }
}