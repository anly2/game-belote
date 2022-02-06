using System.Collections.Generic;
using Belote.Domain;
using Belote.Player.Ai;
using Xunit;
using static AssertNet.Assertions;
using static Belote.Domain.Card;

namespace Belote.Tests.Player.Ai;

public class GameUtilsTest
{
    [Fact]
    public void RecognizesCardPatterns_AllPresent()
    {
        //8♣, J♣, A♥, 9♥, J♥
        IEnumerable<Card> cards = new[] { Clubs_8, Clubs_J, Hearts_9, Hearts_J, Hearts_A };

        var pattern = "J 9 A".ToCardsPattern();

        AssertThat(cards.FindMatchingCards(pattern)).ContainsExactly(Hearts_J, Hearts_9, Hearts_A);
    }

    [Fact]
    public void RecognizesCardPatterns_SomePresent()
    {
        //8♣, J♣, A♥, 9♥, J♥
        IEnumerable<Card> cards = new[] { Clubs_8, Clubs_J, Hearts_9, Hearts_J, Hearts_A };

        var pattern = "J A".ToCardsPattern();

        AssertThat(cards.FindMatchingCards(pattern)).ContainsExactly(Hearts_J, Hearts_A);
    }

    [Fact]
    public void RecognizesCardPatterns_MultipleSuits()
    {
        //8♣, J♣, A♥, 9♥, J♠
        IEnumerable<Card> cards = new[] { Clubs_8, Clubs_J, Hearts_9, Hearts_A, Spades_J };

        var pattern = "J, J, 9 A".ToCardsPattern();

        AssertThat(cards.FindMatchingCards(pattern)).ContainsExactly(Clubs_J, Spades_J, Hearts_9, Hearts_A);
    }

    [Fact]
    public void RecognizesCardPatterns_WithATen()
    {
        //9♦, Q♦, K♦, 10♦, A♦
        IEnumerable<Card> cards = new[] { Diamonds_9, Diamonds_Q, Diamonds_K, Diamonds_10, Diamonds_A };

        var pattern = "A 10 K Q".ToCardsPattern();

        AssertThat(cards.FindMatchingCards(pattern)).ContainsExactly(Diamonds_A, Diamonds_10, Diamonds_K, Diamonds_Q);
    }
}