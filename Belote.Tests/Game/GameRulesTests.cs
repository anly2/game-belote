using Belote.Domain;
using Belote.Game;
using Xunit;
using static AssertNet.Assertions;
using static Belote.Domain.Card;

namespace Belote.Tests.Game;

public class GameRulesTests
{
    [Fact]
    public void CanBePlayed_ForcesTrumping_WhenAskedNotAvailableButATrumpCardIs()
    {
        //Contract: Hearts
        //Trick:    8♦
        //Hand:     7♣, Q♣, 10♣, 7♥, K♥, 10♥, J♥, 9♠
        //expected: x , x ,  x  , ✓ , ✓ ,  ✓ , ✓ , x

        var contract = Contract.Hearts;
        var trick = new[] {Diamonds_8};
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9};
        var expected = new[] {false, false, false, true, true, true, true, false};

        AssertThat(hand).Select(c => c.CanBePlayed(contract, hand, trick))
            .ContainsExactly(expected);

    }
}