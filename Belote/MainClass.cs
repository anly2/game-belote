using System.Collections.Generic;
using System.Linq;
using Belote.Players;
using Belote.Players.Basic;

namespace Belote
{
    public static class MainClass
    {
        public static void Main(string[] args)
        {
            new Game.Game(
                Game.Game.GetPlayingDeck(),
                new List<IPlayer>(Enumerable.Repeat<IPlayer>(new Player(), 4))
            ).PlayGame();
        }
    }
}