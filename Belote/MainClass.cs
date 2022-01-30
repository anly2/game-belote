﻿using System;
using System.Collections.Generic;
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
            var players = new List<IPlayer>(new IPlayer[]{
                new ConsoleHumanPlayer("A", true),
                new ConsoleHumanPlayer("B"),
                new ConsoleHumanPlayer("A"),
                new ConsoleHumanPlayer("B")
                // new BasicAiPlayer(),
                // new BasicAiPlayer(),
                // new BasicAiPlayer()
            });
            new Game.Game(Game.Game.GetPlayingDeck(), players).PlayGame();
        }
    }
}