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
    public void RecognizesCardPatterns()
    {
        //8♣, J♣, 9♥, J♥, A♥
        IEnumerable<Card> cards = new[] { Clubs_8, Clubs_J, Hearts_9, Hearts_J, Hearts_A };

        var pattern = "J 9 A".ToCardsPattern();

        AssertThat(cards.FindMatchingCards(pattern)).ContainsExactly(Hearts_J, Hearts_9, Hearts_A);
    }
}