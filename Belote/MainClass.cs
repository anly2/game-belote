using System;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;
using Belote.Player;
using Belote.Player.Ai.Basic;
using Belote.Player.Human.Console;
using static Belote.Player.Human.Console.ConsoleHumanPlayer;

namespace Belote
{
    public static class MainClass
    {
        public static void Main(string[] args)
        {
            // PlayAsHumanWithBots();
            PlayAllBots();
            // PlayAllBots_PrintMatchScores_ForManyGames();
        }

        public static void PlayAsHumanWithBots()
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

            game.ShuffleBeforeGame();
            game.PlayGame();
        }

        public static void PlayAllBots()
        {
            var players = new List<IPlayer>(new IPlayer[]{
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer()
            });
            var game = new Game.Game(Game.Game.GetPlayingDeck(), players);

            Console.Out.WriteLine("Playing:");
            for(var i = 0; i < 7; i++)
                Console.Out.WriteLine(String.Join("", Enumerable.Repeat(" |   ", i)) + " " + players[i % players.Count]);
            Console.Out.WriteLine("=|====|====|====|====|====|====|============================");

            // game.State.Match!.OnBid += t =>
            // {
            //     // Console.Out.WriteLine($"<{players[t.player]}> hand: {game.State.Match.PlayerCards[t.player].Text()}");
            //     Console.Out.WriteLine($"<{players[t.player]}> " + (t.bid != null ? $"bid: {t.bid}" : "passed"));
            // };
            // game.State.Match!.OnCardPlayed += t => Console.Out.WriteLine($"<{players[t.player]}> played: {t.card.Text()}");

            game.State.Match!.OnBiddingEnd += m => Console.Out.WriteLine(RenderBiddingEnd(m));
            game.State.Match!.OnTrickEnd += t => Console.Out.WriteLine(RenderTrickEnd(players, t.trickCards, t.initiator, t.winner));

            game.State.OnMatchEnd += t => Console.Out.WriteLine("Match ended. Match score: " + String.Join(", ", t.score) + " ; Game score: " + String.Join(", ", t.state.Scores));
            game.State.OnGameEnd += s => Console.Out.WriteLine("Game ended. Game score: " + String.Join(", ", s.Scores));

            game.ShuffleBeforeGame();
            game.PlayGame();
        }


        public static void PlayAllBots_PrintMatchScores_ForManyGames()
        {
            var players = new List<IPlayer>(new IPlayer[]{
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer(),
                new BasicAiPlayer()
            });
            var game = new Game.Game(Game.Game.GetPlayingDeck(), players);

            game.State.OnMatchEnd += t => Console.Out.WriteLine(String.Join(", ", t.score));

            game.ShuffleBeforeGame();
            while (true)
            {
                game.PlayGame();
            }
        }
    }
}