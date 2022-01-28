using System.Collections.Generic;
using Belote.Domain;
using Belote.Game;
using Xunit;
using static AssertNet.Assertions;

namespace Belote.Tests.Game;

public class CardSemanticsTests
{
    private static List<Card> GetTestCardHand()
    {
        return new List<Card>(new []
        {
            Card.Diamonds_8, Card.Diamonds_7, Card.Diamonds_9,
            Card.Spades_A, Card.Spades_J, Card.Spades_10,
            Card.Clubs_K,
            Card.Hearts_Q
        });
    }


    [Fact]
    public void CardHands_CanBeSorted_ByPower_AscSuitAscRank_NoContract()
    {
        var hand = GetTestCardHand();

        hand.SortHand();

        AssertThat(hand).ContainsExactly(
            Card.Clubs_K,
            Card.Diamonds_7, Card.Diamonds_8, Card.Diamonds_9,
            Card.Hearts_Q,
            Card.Spades_J, Card.Spades_10, Card.Spades_A
        );
    }

    [Fact]
    public void CardHands_CanBeSorted_ByPower_AscSuitAscRank_OneTrumpContract()
    {
        var hand = GetTestCardHand();

        hand.SortHand(Contract.Spades);

        AssertThat(hand).ContainsExactly(
            Card.Clubs_K,
            Card.Diamonds_7, Card.Diamonds_8, Card.Diamonds_9,
            Card.Hearts_Q,
            Card.Spades_10, Card.Spades_A, Card.Spades_J
        );
    }
}