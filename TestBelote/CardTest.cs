using Belote.Domain;
using Xunit;
using static AssertNet.Assertions;

namespace TestBelote;

public class CardTest
{
    [Fact]
    public void CardSuitsAreCorrect()
    {
        AssertThat(Card.Clubs_A.Suit()).IsEqualTo(0);
        AssertThat(Card.Diamonds_J.Suit()).IsEqualTo(1);
        AssertThat(Card.Hearts_Q.Suit()).IsEqualTo(2);
        AssertThat(Card.Spades_K.Suit()).IsEqualTo(3);
    }

    [Fact]
    public void CardRanksAreCorrect()
    {
        AssertThat(Card.Clubs_7.Rank()).IsEqualTo(5);
        AssertThat(Card.Diamonds_10.Rank()).IsEqualTo(8);
        AssertThat(Card.Hearts_Q.Rank()).IsEqualTo(10);
        AssertThat(Card.Spades_K.Rank()).IsEqualTo(11);
    }
}