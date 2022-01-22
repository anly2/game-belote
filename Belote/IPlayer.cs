using System.Collections.Generic;

namespace Belote
{
    public interface IPlayer
    {
        void BindStateView(IPlayerStateView playerStateView);

        Contract? Bid();
        
        Card Play(List<Declaration> declarations);
    }
}