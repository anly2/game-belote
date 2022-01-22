using System;
using System.Collections.Generic;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Player.Ai.Basic
{
    public class BasicAiPlayer : IPlayer
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