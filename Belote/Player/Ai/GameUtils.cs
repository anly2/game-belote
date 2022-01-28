using System;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;

namespace Belote.Player.Ai
{
    public static class GameUtils
    {
        public static List<Card>? MatchRanks(this IEnumerable<Card> hand, IEnumerable<Card> targets, int[]? suitMapping = null)
        {
            return hand.MatchRanks(targets.Select(c => new CardSelector(c)), suitMapping);
        }

        public static List<Card>? MatchRanks(this IEnumerable<Card> hand, IEnumerable<CardSelector> targets, int[]? suitMapping = null)
        {
            var suits = suitMapping ?? new[] {-1, -1, -1, -1};

            // ReSharper disable PossibleMultipleEnumeration
            var result = new List<Card>(targets.Count());
            foreach (var selector in targets)
            {
                var found = hand.FindCard(selector, suits);
                if (found == null) return null;
                if (selector.Suit >= 0) suits[selector.Suit] = found.Value.Suit();
                result.Add(found.Value);
            }

            return result;
        }


        public static Card? FindCard(this IEnumerable<Card> hand, Card target, int[] suitRemapping)
        {
            return hand.FindCard(CardSelector.CreatePredicate(target.Rank(), target.Suit()), suitRemapping);
        }

        public static Card? FindCard(this IEnumerable<Card> hand, CardSelector selector, int[] suitRemapping)
        {
            return hand.FindCard(selector.Predicate, suitRemapping);
        }

        public static Card? FindCard(this IEnumerable<Card> hand, Func<int, int, int[], bool> selector, int[] suitRemapping)
        {
            return hand.FirstOrDefault(c => selector(c.Rank(), c.Suit(), suitRemapping));
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
    }
}