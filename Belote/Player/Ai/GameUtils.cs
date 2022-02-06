using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Belote.Domain;

namespace Belote.Player.Ai
{
    public static class GameUtils
    {
        public static List<Card>? MatchRanks(this IEnumerable<Card> hand, IEnumerable<Card> targets, int[]? suitMapping = null)
        {
            return hand.FindMatchingCards(targets.Select(c => new CardSelector(c)), suitMapping);
        }

        //ASSUMING the '2' cards are not actually used in the game, use them as a token to represent 'empty' so Rank==0
        private static readonly Card[] EmptyTokensPerSuit = {Card.Clubs_2, Card.Diamonds_2, Card.Hearts_2, Card.Spades_2};
        public static List<Card>? FindMatchingCards(this IEnumerable<Card> hand, IEnumerable<CardSelector> targets, int[]? suitMapping = null)
        {
            //init suit mapping
            var suitRemapping = suitMapping ?? new[] {-1, -1, -1, -1};

            //split by suit
            var bySuit = new List<Card>[4];
            for (var i = 0; i < bySuit.Length; i++)
                bySuit[i] = new List<Card>();
            foreach (var grouping in hand.GroupBy(card => card.Suit()))
                bySuit[grouping.Key].AddRange(grouping);

            //add "empty" tokens
            for (var suit = 0; suit < bySuit.Length; suit++)
            {
                if (bySuit[suit].Count == 0)
                    bySuit[suit].Add(EmptyTokensPerSuit[suit]);
                else
                    //guessing: sort in desc, since patterns are likely to target the strongest cards first (and hands are already in asc order)
                    bySuit[suit].Reverse();
            }


            var result = new List<Card>(targets.Count());
            foreach (var selector in targets)
            {
                var haystack = selector.Suit == -1 ? bySuit.SelectMany(g => g) :
                    suitRemapping[selector.Suit] == -1 ? suitRemapping.Where(s => s == -1).SelectMany((_,i) => bySuit[i]) :
                    bySuit[suitRemapping[selector.Suit]];

                //FIXME: this does not work when backtracking is required (pattern: J 9, hand: J♣, 9♥, J♥, A♥  --> wrongly goes for ♣
                Card? found = haystack.FirstOrDefault(c => selector.Predicate(c.Rank(), c.Suit(), suitRemapping));
                if (found == null) return null;
                result.Add(found.Value);
                var suit = found.Value.Suit();
                bySuit[suit].Remove(found.Value);
                if (selector.Suit >= 0) suitRemapping[selector.Suit] = suit;
            }

            return result;
        }


        public class CardSelector
        {
            public readonly int Suit;
            public readonly Func<int, int, int[], bool> Predicate;

            public CardSelector(Func<int, int, int[], bool> predicate, int suit = -1)
            {
                Predicate = predicate;
                Suit = suit;
            }

            public CardSelector(Card target) : this(target.Rank(), target.Suit()) {}

            public CardSelector(int targetRank, int targetSuit) : this(CreatePredicate(targetRank, targetSuit), targetSuit) {}

            public static Func<int, int, int[], bool> CreatePredicate(int targetRank, int targetSuit)
            {
                return (rank, suit, suitRemapping) =>
                    (targetRank < 0 || rank == targetRank)
                    && (targetSuit < 0 || suitRemapping[targetSuit] < 0 || suit == suitRemapping[targetSuit]);
            }
        }


        public static IEnumerable<CardSelector> ToCardsPattern(this string expression)
        {
            return expression.Split(",", StringSplitOptions.TrimEntries)
                .SelectMany(ParseSingleSuitCardsPattern);
        }

        private static readonly Regex CardPatternExpression = new Regex(
            "(?:(?<wildcard>\\*)|(?<operator><|<=|>|>=)?(?<rank>[AKQJ]|\\d+))(?<optional>\\?)?"
            , RegexOptions.Compiled);
        private static IEnumerable<CardSelector> ParseSingleSuitCardsPattern(string expression, int suit)
        {
            return CardPatternExpression.Matches(expression).Select(match =>
            {
                Func<int, bool> predicate;
                if (match.Groups["wildcard"].Success)
                {
                    if (match.Groups["optional"].Success)
                        predicate = rank => true;
                    else
                        predicate = rank => rank > 0;
                }
                else
                {
                    var targetRank = Enum.Parse<Card>("Clubs_" + match.Groups["rank"].Value.ToUpper()).Rank();
                    predicate = match.Groups["operator"].Value switch
                    {
                        "<=" => rank => rank <= targetRank && rank > 0,
                        "<" => rank => rank < targetRank && rank > 0,
                        ">=" => rank => rank >= targetRank,
                        ">" => rank => rank > targetRank,
                        _ => rank => rank == targetRank
                    };

                    if (match.Groups["optional"].Success)
                    {
                        var org = predicate;
                        predicate = rank => rank == 0 || org(rank);
                    }
                }

                return new CardSelector((rank, _, __) => predicate(rank), suit);
            });
        }
    }
}