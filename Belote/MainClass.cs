using System.Collections.Generic;
using System.Linq;
using Belote.Player;
using Belote.Player.Ai.Basic;

namespace Belote
{
    public static class MainClass
    {
        public static void Main(string[] args)
        {
            new Game.Game(
                Game.Game.GetPlayingDeck(),
                new List<IPlayer>(Enumerable.Repeat<IPlayer>(new BasicAiPlayer(), 4))
            ).PlayGame();
        }
    }
}