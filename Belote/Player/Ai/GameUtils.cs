using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Belote.Domain;
using Belote.Game;
using static Belote.Domain.CardUtils;

namespace Belote.Player.Ai
{
    public static class GameUtils
    {
        private const char TextualRankStart = 'a';
        public static List<Card> FindMatchingCards(this IEnumerable<Card> hand, IEnumerable<Regex> patterns)
        {
            var bySuit = new string?[4];
            for (var i = 0; i < bySuit.Length; i++)
            {
                var j = i;
                bySuit[i] = String.Join("", hand.Where(c => c.Suit() == j)
                    .OrderByDescending(c => c.PowerWhenTrump())
                    .Select(c => c.Rank())
                    .Select(r => (char) (TextualRankStart + r)));
            }


            var result = new List<Card>();
            foreach (var pattern in patterns)
            {
                for (var i = 0; i < bySuit.Length; i++)
                {
                    if (bySuit[i] == null) continue;
                    var match = pattern.Match(bySuit[i]);
                    if (!match.Success) continue;
                    result.AddRange(match.Groups.Values.Skip(1).Select(m => GetCard(m.Value[0] - TextualRankStart, i)));
                    bySuit[i] = null;
                }
            }

            return result;
        }


        public static IEnumerable<Regex> ToCardsPattern(this string expression)
        {
            return expression.Split(",", StringSplitOptions.TrimEntries)
                .Select(ParseSingleSuitCardsPattern);
        }

        private static readonly Regex CardPatternExpression = new Regex(
            "(?:(?<wildcard>\\*)|(?<operator><|<=|>|>=)?(?<rank>[AKQJ]|\\d+))(?<optional>\\?)?"
            , RegexOptions.Compiled);

        private static readonly IEnumerable<int> AllCardRanks = GetAllCardRanks();

        private static Regex ParseSingleSuitCardsPattern(string expression)
        {
            return new Regex(String.Join("", CardPatternExpression.Matches(expression).Select(match =>
            {
                IEnumerable<int> targetRanks;
                if (match.Groups["wildcard"].Success)
                {
                    targetRanks = AllCardRanks;
                }
                else
                {
                    var anchorRank = Enum.Parse<Card>("Clubs_" + match.Groups["rank"].Value.ToUpper()).Rank();
                    targetRanks = AllCardRanks.Where(match.Groups["operator"].Value switch
                    {
                        "<=" => rank => rank <= anchorRank,
                        "<" => rank => rank < anchorRank,
                        ">=" => rank => rank >= anchorRank,
                        ">" => rank => rank > anchorRank,
                        _ => rank => rank == anchorRank
                    });
                }

                var res = "[" + String.Join("", targetRanks.Select(r => (char) (TextualRankStart + r))) + "]";
                if (match.Groups["optional"].Success)
                    res += (res.Length == 0 ? "" : "|") + "^$";
                return "("+res+")";
            })), RegexOptions.Compiled);
        }
    }
}