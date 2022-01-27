using System.Collections.Generic;
using System.Linq;
using Belote.Domain;

namespace Belote.Game
{
    public static class BeloteCardSemantics
    {
        private static readonly int[] CardPowers =
        {
            // 2,  3,  4,  5,  6, 7, 8, 9, 10, J, Q, K, A
            -1, -1, -1, -1, -1, 0, 1, 2,  6, 3, 4, 5, 7
        };
        
        private static readonly int[] CardTrumpPowers =
        {
            // 2,  3,  4,  5,  6, 7, 8, 9, 10, J, Q, K, A
            -1, -1, -1, -1, -1, 0, 1, 8,  6, 9, 4, 5, 7
        };
        
        private static readonly int[] CardValues =
        {
            //7, 8, 9,  J,  Q, K, 10, A,  T9, TJ 
            0, 0, 0,  2,  3, 4, 10, 11, 14, 20
        };
        
        public static int Power(this Card card) => CardPowers[card.Rank()];
        public static int PowerWhenTrump(this Card card) => CardTrumpPowers[card.Rank()];
        
        public static int Value(this Card card) => CardValues[card.Power()];
        public static int ValueWhenTrump(this Card card) => CardValues[card.PowerWhenTrump()];


        public static bool IsTrump(this Card card, Contract? contract = null)
        {
            return contract?.Plain() switch
            {
                null => false,
                Contract.NoTrumps => false,
                Contract.AllTrumps => true,
                _ => ContractUtils.PlainContracts[card.Suit()] == contract
            };
        }


        public static void SortHand(this List<Card> hand, Contract? contract = null)
        {
            hand.Sort((a,b) =>
            {
                var suitComparison = a.Suit().CompareTo(b.Suit());
                return suitComparison != 0 ? suitComparison : -(a.IsTrump(contract)
                        ? a.PowerWhenTrump().CompareTo(b.PowerWhenTrump())
                        : a.Power().CompareTo(b.Power()));
            });
        }
    }
}