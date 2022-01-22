using System.Collections.Generic;
using Belote.Domain;
using Belote.Game.State;

namespace Belote.Player
{
    public interface IPlayer
    {
        void BindStateView(IPlayerStateView playerStateView);

        Contract? Bid();
        
        Card Play(List<Declaration> declarations);
    }
}