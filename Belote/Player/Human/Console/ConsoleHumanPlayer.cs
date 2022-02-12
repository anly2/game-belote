using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Belote.Domain;
using Belote.Game;
using Belote.Game.State;
using static Belote.Domain.ContractUtils;

namespace Belote.Player.Human.Console
{
    public class ConsoleHumanPlayer : NamedPlayer
    {
        private IPlayerStateView? _state;

        public ConsoleHumanPlayer(string? name = null)
        {
            Name = name;
        }

        public override void BindStateView(IPlayerStateView playerStateView)
        {
            base.BindStateView(playerStateView);

            _state = playerStateView;
        }


        public override Contract? Bid()
        {
            Print("Your turn to bid:");
            Print("Cards: " + _state!.CurrentHand.Text());

            PrintBidOptions();
            if (!int.TryParse(System.Console.ReadLine(), out var bid)) bid = 0;
            bid--;

            if (bid == 6)
            {
                return _state.CurrentContract?.IsContre() == true ?
                    _state.CurrentContract?.Recontra() : _state.CurrentContract?.Contre();
            }

            if (bid < 0) return null;
            return PlainContracts[bid];
        }

        private void PrintBidOptions()
        {
            var currentContract = (int?) _state?.CurrentContract ?? -1;

            var options = new[] { ("pass", true) }.Concat(
                PlainContracts.Select(contract => (contract.Text(), (int) contract > currentContract)) ).ToList();

            if (currentContract >= 0 && !_state!.IsCommittedToCurrentContract)
            {
                var c = (Contract) currentContract;
                options.Add( ((c.IsPlain() ? c.Contre() : c.Recontra()).Text(), true) );
            }

            PrintOptions(options.ToArray());
        }

        public override Card Play(List<Declaration> declarations)
        {
            var contract = _state!.CurrentContract!.Value;
            var hand = _state!.CurrentHand;
            var trick = _state!.CurrentTrick;

            Print($"Your turn to play : ({contract})  {trick.Text()}");
            Print("Cards: " + hand.Text());

            var playableCards = hand.PlayableCards(contract, trick).ToList();
            PrintOptions(1, playableCards.Select(c => (c.Text(), true)).ToArray());

            if (playableCards.Count == 1)
            {
                System.Console.ReadLine(); //anything
                return playableCards[0];
            }

            if (!int.TryParse(System.Console.ReadLine(), out var choice))
                throw new ArgumentException("Invalid choice!");
            choice--;

            return playableCards[choice];
        }



        private static void PrintOptions(params (string text, bool available)[] options)
        {
            PrintOptions(0, options);
        }
        private static void PrintOptions(int start, params (string text, bool available)[] options)
        {
            var sb = new StringBuilder();
            sb.Append('(');

            var n = start;
            foreach (var (text, available) in options)
            {
                sb.Append(' ').Append(available ? n.ToString() : "-").Append(':').Append(text);
                n++;
            }

            sb.Append(" )");
            System.Console.Out.WriteLine(sb.ToString());
        }


        public static string RenderBid(int player, Contract? contract)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < player; i++)
                sb.Append("     ");

            if (contract == null)
                sb.Append("pass");
            else
                sb.Append(' ').Append(contract.Value.Text());

            return sb.ToString();
        }

        public static string RenderBiddingEnd(IMatchState match)
        {
            if (match.Contract == null) return "everyone passed";
            return RenderBid(match.CommittedPlayer ?? 0, match.Contract);
        }

        public static string RenderTrickEnd(IList players, IEnumerable<Card> trickCards, int initiator, int winner)
        {
            // System.Console.Out.WriteLine($"<{players[winner]}> won a trick: {trickCards.Text()}");
            var sb = new StringBuilder();

            for (var i = 0; i < initiator; i++)
                sb.Append("     ");

            {
                var i = initiator;
                foreach (var trickCard in trickCards)
                {
                    var cardText = trickCard.Text();
                    sb.Append(' ').Append(cardText).Append(' ');

                    //pad to 3 chars
                    var d = 3 - cardText.Length;
                    for (var j = 0; j < d; j++) sb.Append(' ');

                    if (winner == i)
                    {
                        sb.Replace(' ', '(', sb.Length - 5, 1);
                        sb.Replace(' ', ')', sb.Length - 1 - d, 1);
                    }
                    i = (i + 1) % players.Count;
                }
            }

            return sb.ToString();
        }

        public static string RenderScore(IReadOnlyList<byte> gameScore, int[] matchScore, Contract? contract)
        {
            // return "Match score: " + String.Join(", ", matchScore) + " ; Game score: " + String.Join(", ", gameScore);
            var sb = new StringBuilder();
            sb.Append("Match score: ").Append(String.Join(", ", matchScore));
            sb.Append(" ; ");

            IEnumerable<string> gameScoreEntries;
            if (contract != null)
            {
                var points = Game.Game.MatchScoreToGameScore(matchScore, contract.Value);
                gameScoreEntries = gameScore.Select((s, i) => $"{s} (+{points[i]})");
            }
            else
            {
                gameScoreEntries = gameScore.Select(b => b.ToString());
            }

            sb.Append("Game score: ").Append(String.Join(", ", gameScoreEntries));
            return sb.ToString();
        }
    }
}