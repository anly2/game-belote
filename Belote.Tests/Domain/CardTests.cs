using AssertNet;
using Belote.Domain;
using Xunit;

namespace Belote.Tests.Domain;

public class CardTests
{
    [Fact]
    public void CardSuitsAreCorrect()
    {
        Assertions.AssertThat(Card.Clubs_A.Suit()).IsEqualTo(0);
        Assertions.AssertThat(Card.Diamonds_J.Suit()).IsEqualTo(1);
        Assertions.AssertThat(Card.Hearts_Q.Suit()).IsEqualTo(2);
        Assertions.AssertThat(Card.Spades_K.Suit()).IsEqualTo(3);
    }

    [Fact]
    public void CardRanksAreCorrect()
    {
        Assertions.AssertThat(Card.Clubs_7.Rank()).IsEqualTo(5);
        Assertions.AssertThat(Card.Diamonds_10.Rank()).IsEqualTo(8);
        Assertions.AssertThat(Card.Hearts_Q.Rank()).IsEqualTo(10);
        Assertions.AssertThat(Card.Spades_K.Rank()).IsEqualTo(11);
    }
}