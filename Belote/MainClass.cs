using System;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;
using Belote.Player;
using Belote.Player.Ai.Basic;
using Belote.Player.Human.Console;

namespace Belote
{
    public static class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Out.WriteLine("Hearts_7: " + Card.Hearts_7.Text() + " " + Card.Hearts_7.Rank() + " / " + Card.Hearts_7.Suit());
            Console.Out.WriteLine("Hearts_7: " + Card.Spades_A.Text() + " " + Card.Spades_A.Rank() + " / " + Card.Spades_A.Suit());
            var players = new List<IPlayer>(new IPlayer[]{
                new ConsoleHumanPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer()
            });
            new Game.Game(Game.Game.GetPlayingDeck(), players).PlayGame();
        }
    }
}