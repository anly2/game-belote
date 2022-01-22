using System;
using System.Collections.Generic;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Players.Basic
{
    public class Player : IPlayer
    {
        private IPlayerStateView? State;
        
        public void BindStateView(IPlayerStateView playerStateView)
        {
            State = playerStateView;
        }

        public Contract? Bid()
        {
            throw new NotImplementedException();
        }

        public Card Play(List<Declaration> declarations)
        {
            throw new NotImplementedException();
        }
    }
}