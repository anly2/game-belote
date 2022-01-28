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

    [Fact]
    public void CardComparison_SameSuit_HigherRank_NotTrumps_Wins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Diamonds_J, Contract.Spades) > 0);
        AssertThat(Card.Diamonds_J.CompareTo(Card.Diamonds_Q, Contract.Spades) < 0);
    }

    [Fact]
    public void CardComparison_SameSuit_HigherRank_WhenTrumps_Wins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Diamonds_J, Contract.Diamonds) < 0);
        AssertThat(Card.Diamonds_J.CompareTo(Card.Diamonds_Q, Contract.Diamonds) > 0);
    }

    [Fact]
    public void CardComparison_DifferentSuits_NoTrumps_FirstWins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.NoTrumps) > 0);
        AssertThat(Card.Spades_A.CompareTo(Card.Diamonds_Q, Contract.NoTrumps) > 0);
    }

    [Fact]
    public void CardComparison_DifferentSuits_BothTrumps_FirstWins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.AllTrumps) > 0);
        AssertThat(Card.Spades_A.CompareTo(Card.Diamonds_Q, Contract.AllTrumps) > 0);
    }

    [Fact]
    public void CardComparison_DifferentSuits_OneIsTrump_TrumpWins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.Diamonds) > 0);
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.Spades) < 0);
        AssertThat(Card.Spades_A.CompareTo(Card.Diamonds_Q, Contract.Diamonds) < 0);
    }
}