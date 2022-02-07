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
            // PlayWithBots();
            LetBotsPlay();
        }

        public static void PlayWithBots()
        {
            var players = new List<IPlayer>(new IPlayer[]{
                new ConsoleHumanPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer()
            });
            var game = new Game.Game(Game.Game.GetPlayingDeck(), players);
            game.State.Match!.OnBid += t => Console.Out.WriteLine($"<{players[t.player]}> " + (t.bid != null ? $"bid: {t.bid}" : "passed"));
            game.State.Match!.OnCardPlayed += t => Console.Out.WriteLine($"<{players[t.player]}> played: {t.card.Text()}");
            game.State.Match!.OnTrickEnd += t => Console.Out.WriteLine($"<{players[t.winner]}> won a trick: {t.trickCards.Text()}");
            game.State.OnMatchEnd += t => Console.Out.WriteLine("Match ended. Match score: " + String.Join(", ", t.score));
            game.State.OnGameEnd += s => Console.Out.WriteLine("Game ended. Score: " + String.Join(", ", s.Scores));
            game.PlayGame();
        }

        public static void LetBotsPlay()
        {
            var players = new List<IPlayer>(new IPlayer[]{
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer()
            });
            var game = new Game.Game(Game.Game.GetPlayingDeck(), players);
            game.State.Match!.OnBid += t =>
            {
                // Console.Out.WriteLine($"<{players[t.player]}> hand: {game.State.Match.PlayerCards[t.player].Text()}");
                Console.Out.WriteLine($"<{players[t.player]}> " + (t.bid != null ? $"bid: {t.bid}" : "passed"));
            };
            // game.State.Match!.OnCardPlayed += t => Console.Out.WriteLine($"<{players[t.player]}> played: {t.card.Text()}");
            game.State.Match!.OnTrickEnd += t => Console.Out.WriteLine($"<{players[t.winner]}> won a trick: {t.trickCards.Text()}");
            game.State.OnMatchEnd += t => Console.Out.WriteLine("Match ended. Match score: " + String.Join(", ", t.score));
            game.State.OnGameEnd += s => Console.Out.WriteLine("Game ended. Game score: " + String.Join(", ", s.Scores));
            game.PlayGame();
        }
    }
}