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
    }
}