using System;
using System.Collections.Generic;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Player.Human.Console
{
    public class ConsoleHumanPlayer : IPlayer
    {
        private IPlayerStateView? _state;

        public void BindStateView(IPlayerStateView playerStateView)
        {
            _state = playerStateView;
        }

        protected void Print(string message)
        {
            System.Console.Out.WriteLine("[Player " + (_state?.PlayerIndex + 1) + "] " + message);
        }


        public Contract? Bid()
        {
            Print("Your turn to bid:");
            Print("Cards: " + _state?.CurrentHand.Text());

            System.Console.Out.WriteLine("(0:pass 1:♣ 2:♦ 3:♥ 4:♠ 5:A 6:J 7:X 8:XX)");
            var bid = Convert.ToInt32(System.Console.ReadLine()) - 1;
            if (bid < 0) return null;
            return (Contract) bid;
        }

        public Card Play(List<Declaration> declarations)
        {
            return _state.CurrentHand[0];
        }
    }
}