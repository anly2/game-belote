using System.Collections.Generic;
using System.Linq;
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

        hand.SortHand(null);

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
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Diamonds_J, Contract.Spades)).IsGreaterThan(0);
        AssertThat(Card.Diamonds_J.CompareTo(Card.Diamonds_Q, Contract.Spades)).IsLesserThan(0);
    }

    [Fact]
    public void CardComparison_SameSuit_HigherRank_WhenTrumps_Wins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Diamonds_J, Contract.Diamonds)).IsLesserThan(0);
        AssertThat(Card.Diamonds_J.CompareTo(Card.Diamonds_Q, Contract.Diamonds)).IsGreaterThan(0);
    }

    [Fact]
    public void CardComparison_DifferentSuits_NoTrumps_FirstWins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.NoTrumps)).IsGreaterThan(0);
        AssertThat(Card.Spades_A.CompareTo(Card.Diamonds_Q, Contract.NoTrumps)).IsGreaterThan(0);
    }

    [Fact]
    public void CardComparison_DifferentSuits_BothTrumps_FirstWins()
    {
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.AllTrumps)).IsGreaterThan(0);
        AssertThat(Card.Spades_A.CompareTo(Card.Diamonds_Q, Contract.AllTrumps)).IsGreaterThan(0);
    }

    [Fact]
    public void CardComparison_DifferentSuits_OneIsTrump_TrumpWins()
    {
        AssertThat(Card.Hearts_K.CompareTo(Card.Diamonds_Q, Contract.Hearts)).IsGreaterThan(0);
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Hearts_K, Contract.Hearts)).IsLesserThan(0);

        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.Diamonds)).IsGreaterThan(0);
        AssertThat(Card.Diamonds_Q.CompareTo(Card.Spades_A, Contract.Spades)).IsLesserThan(0);
        AssertThat(Card.Spades_A.CompareTo(Card.Diamonds_Q, Contract.Diamonds)).IsLesserThan(0);
    }


    [Fact]
    public void StrongestCard_ValuesNonTrumpCardsCorrectly()
    {
        AssertThat(new[] {Card.Clubs_J, Card.Hearts_8, Card.Clubs_10, Card.Clubs_A}.StrongestCard(Contract.NoTrumps))
            .IsEqualTo(Card.Clubs_A);
    }

    [Fact]
    public void StrongestCard_ValuesTrumpCardsCorrectly()
    {
        AssertThat(new[] {Card.Clubs_J, Card.Hearts_8, Card.Clubs_10, Card.Diamonds_A}.StrongestCard(Contract.AllTrumps))
            .IsEqualTo(Card.Clubs_J);
    }

    [Fact]
    public void StrongestCard_HonorsTrumping()
    {
        AssertThat(new[] {Card.Clubs_J, Card.Hearts_8, Card.Clubs_10, Card.Diamonds_A}.StrongestCard(Contract.Hearts))
            .IsEqualTo(Card.Hearts_8);
    }

    [Fact]
    public void StrongestCard_HonorsAskedSuit()
    {
        // NoTrumps: J♠, 8♥, 10♠, A♦  --  should pick 10♠ and not A♦
        AssertThat(new[] {Card.Clubs_J, Card.Hearts_8, Card.Clubs_10, Card.Diamonds_A}.StrongestCard(Contract.NoTrumps))
            .IsEqualTo(Card.Clubs_10);
    }
}