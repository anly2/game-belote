using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Belote.Domain;

namespace Belote.Game
{
    public static class GameRules
    {
        public static bool CanBePlayed(this Card card, Contract contract, IEnumerable<Card> handCards, IEnumerable<Card> trickCards)
        {
            return handCards.PlayableCards(contract, trickCards).Contains(card);
        }

        public static IEnumerable<Card> PlayableCards(this IEnumerable<Card> handCards, Contract contract, IEnumerable<Card> trickCards)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (!trickCards.Any()) return handCards;

            var askedCard = trickCards.First();

            // If the asked suit can be answered, a card of that suit must be played
            var askedSuit = askedCard.Suit();
            var askedIsNotTrump = !askedCard.IsTrump(contract);
            var sameSuitCards = handCards.Where(c => askedSuit == c.Suit());
            if (sameSuitCards.Any())
            {
                if (askedIsNotTrump) return sameSuitCards;

                // If playing a trump suit, a stronger card must be played, if available
                var strongest = trickCards.StrongestCard(contract);
                return sameSuitCards.Where(c => strongest.CompareTo(c, contract) < 0);
            }

            // If the asked suit cannot be answered, and the current winner is not a teammate, a trump must be played, if available
            if (askedIsNotTrump)
            {
                var trumpCards = handCards.Where(c => c.IsTrump(contract));
                if (trumpCards.Any() && !StrongestIsTeammate(trickCards, contract))
                    return trumpCards;
            }

            // If cannot answer asked and cannot trump, can play anything but would lose it
            return handCards;
        }

        public static bool StrongestIsTeammate(IEnumerable<Card> trickCards, Contract contract)
        {
            //Assumes 4 players
            return trickCards.SkipLast(1).LastOrDefault() == trickCards.StrongestCard(contract);
        }
    }
}