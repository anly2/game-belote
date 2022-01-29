using System.Collections.Generic;
using System.Linq;
using Belote.Domain;

namespace Belote.Game
{
    public static class GameRules
    {
        public static bool CanBePlayed(this Card card, Contract contract, IEnumerable<Card> handCards, IEnumerable<Card> trickCards)
        {
            // ReSharper disable PossibleMultipleEnumeration
            if (!trickCards.Any()) return true;

            var askedCard = trickCards.First();
            var askedSuit = askedCard.Suit();
            var askedIsNotTrump = !askedCard.IsTrump(contract);

            if (askedSuit != card.Suit())
            {
                // has a card of the asked suit
                if (handCards.Any(c => askedSuit == c.Suit()))
                    return false;

                // can overrule with trump
                if (askedIsNotTrump && handCards.Any(c => c.IsTrump(contract)))
                    return card.IsTrump(contract);

                return true;
            }

            // Same suit
            if (askedIsNotTrump) return true;

            // (card > strongest) OR no stronger card in hand
            var strongest = trickCards.StrongestCard(contract);
            return (card.CompareTo(strongest, contract) > 0)
                   || handCards.All(c => c == card || strongest.CompareTo(c, contract) > 0);
        }
    }
}