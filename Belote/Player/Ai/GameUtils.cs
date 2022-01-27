using System.Collections.Generic;
using System.Linq;
using Belote.Domain;

namespace Belote.Player.Ai
{
    public static class GameUtils
    {
        public static List<Card>? MatchRanks(this IEnumerable<Card> hand, IEnumerable<Card> target, int[]? suitMapping = null)
        {
            var suits = suitMapping ?? new[] {-1, -1, -1, -1};

            // ReSharper disable PossibleMultipleEnumeration
            var result = new List<Card>(target.Count());
            foreach (var card in target)
            {
                var found = hand.FindCard(card, suits);
                if (found == null) return null;
                suits[card.Suit()] = found.Value.Suit();
                result.Add(found.Value);
            }

            return result;
        }

        public static Card? FindCard(this IEnumerable<Card> hand, Card target, int[] suitRemapping)
        {
            var targetRank = target.Rank();
            var targetSuit = suitRemapping[target.Suit()];
            return hand.FirstOrDefault(c => c.Rank() == targetRank && (targetSuit < 0 || c.Suit() == targetSuit) );
        }
    }
}