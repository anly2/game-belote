using Belote.Domain;
using Belote.Game;
using Xunit;
using static AssertNet.Assertions;
using static Belote.Domain.Card;

namespace Belote.Tests.Game;

public class GameRulesTests
{
    [Fact]
    public void PlayableCards_AllowsAnything_WhenNothingAskedYet()
    {
        //Contract: AllTrumps
        //Trick:    -
        //Hand:     7♣, Q♣, 10♣, 7♥, K♥, 10♥, J♥, 9♠
        //expected: ✓ , ✓ ,  ✓  , ✓ , ✓ ,  ✓ , ✓ , ✓

        var contract = Contract.AllTrumps;
        var trick = System.Array.Empty<Card>();
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9);
    }

    [Fact]
    public void PlayableCards_ForcesSameSuit_WhenAskedIsAvailable_AnyIfNotTrumpSuit()
    {
        //Contract: Hearts
        //Trick:    8♦
        //Hand:     7♣, Q♣, 10♣, 7♦, K♥, 10♥, J♥, 9♠
        //expected: x , x ,  x  , ✓ , x ,  x , x , x

        var contract = Contract.Hearts;
        var trick = new[] {Diamonds_8};
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Diamonds_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Diamonds_7);
    }

    [Fact]
    public void PlayableCards_ForcesSameSuit_WhenAskedIsAvailable_StrongerIfTrumpSuit()
    {
        //Contract: AllTrumps
        //Trick:    8♦
        //Hand:     7♣, Q♣, 10♣, 7♦, K♦, 10♥, J♥, 9♠
        //expected: x , x ,  x  , x , ✓ ,  x , x , x

        var contract = Contract.AllTrumps;
        var trick = new[] {Diamonds_8};
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Diamonds_7, Diamonds_K, Hearts_10, Hearts_J, Clubs_9};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Diamonds_K);
    }

    [Fact]
    public void PlayableCards_ForcesTrumping_WhenAskedNotAvailableButATrumpCardIs_AndCurrentWinnerIsAnOpponent()
    {
        //Contract: Hearts
        //Trick:    8♦
        //Hand:     7♣, Q♣, 10♣, 7♥, K♥, 10♥, J♥, 9♠
        //expected: x , x ,  x  , ✓ , ✓ ,  ✓ , ✓ , x

        var contract = Contract.Hearts;
        var trick = new[] {Diamonds_8};
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Hearts_7, Hearts_K, Hearts_10, Hearts_J);
    }

    [Fact]
    public void PlayableCards_AllowsAnything_WhenAskedNotAvailableButATrumpCardIs_AndCurrentWinnerIsTeammate()
    {
        //Contract: Hearts
        //Trick:    8♦, 7♦
        //Hand:     7♣, Q♣, 10♣, 7♥, K♥, 10♥, J♥, 9♠
        //expected: ✓ , ✓ ,  ✓  , ✓ , ✓ ,  ✓ , ✓ , ✓

        var contract = Contract.Hearts;
        var trick = new[] {Diamonds_8, Diamonds_7};
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9);
    }

    [Fact]
    public void PlayableCards_AllowsAnything_WhenCannotMeetSuit()
    {
        //Contract: AllTrumps
        //Trick:    8♦
        //Hand:     7♣, Q♣, 10♣, 7♥, K♥, 10♥, J♥, 9♠
        //expected: ✓ , ✓ ,  ✓  , ✓ , ✓ ,  ✓ , ✓ , ✓

        var contract = Contract.AllTrumps;
        var trick = new[] {Diamonds_8};
        var hand = new[] {Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Spades_7, Spades_Q, Spades_10, Hearts_7, Hearts_K, Hearts_10, Hearts_J, Clubs_9);
    }

    [Fact]
    public void PlayableCards_AllowsAnyTrump_WhenCannotRaise()
    {
        //Contract: Hearts
        //Trick:    J♥
        //Hand:     K♦, K♥, 10♥, 8♠, Q♠
        //expected: x , ✓ ,  ✓  , x , x

        var contract = Contract.Hearts;
        var trick = new[] {Hearts_J};
        var hand = new[] {Diamonds_K, Hearts_K, Hearts_10, Spades_8, Spades_Q};

        AssertThat(hand.PlayableCards(contract, trick))
            .ContainsExactly(Hearts_K, Hearts_10);
    }
}