﻿namespace Belote
{
    public enum Card
    {
        Clubs_2,
        Clubs_3,
        Clubs_4,
        Clubs_5,
        Clubs_6,
        Clubs_7,
        Clubs_8,
        Clubs_9,
        Clubs_10,
        Clubs_J,
        Clubs_Q,
        Clubs_K,
        Clubs_A,
        Diamonds_2,
        Diamonds_3,
        Diamonds_4,
        Diamonds_5,
        Diamonds_6,
        Diamonds_7,
        Diamonds_8,
        Diamonds_9,
        Diamonds_10,
        Diamonds_J,
        Diamonds_Q,
        Diamonds_K,
        Diamonds_A,
        Hearts_2,
        Hearts_3,
        Hearts_4,
        Hearts_5,
        Hearts_6,
        Hearts_7,
        Hearts_8,
        Hearts_9,
        Hearts_10,
        Hearts_J,
        Hearts_Q,
        Hearts_K,
        Hearts_A,
        Spades_2,
        Spades_3,
        Spades_4,
        Spades_5,
        Spades_6,
        Spades_7,
        Spades_8,
        Spades_9,
        Spades_10,
        Spades_J,
        Spades_Q,
        Spades_K,
        Spades_A
    }
    
    
    public static class CardUtils
    {
        public static int Suit(this Card card)
        {
            return (int) card / 14;
        }
        
        public static int Rank(this Card card)
        {
            return (int) card % 14;
        }
    }
}