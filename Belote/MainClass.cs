using System;
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
                new ConsoleHumanPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer()
                // new ConsoleHumanPlayer(),
                // new ConsoleHumanPlayer(),
                // new ConsoleHumanPlayer()
            });
            var game = new Game.Game(Game.Game.GetPlayingDeck(), players);
            game.State.Match!.OnBid += (t) => Console.Out.WriteLine($"<{players[t.player]}> " + (t.bid != null ? $"bid: {t.bid}" : "passed"));
            game.State.Match!.OnCardPlayed += (t) => Console.Out.WriteLine($"<{players[t.player]}> played: {t.card.Text()}");
            game.State.Match!.OnTrickEnd += t => Console.Out.WriteLine($"<{players[t.winner]}> won a trick: {t.trickCards.Text()}");
            game.PlayGame();
        }
    }
}