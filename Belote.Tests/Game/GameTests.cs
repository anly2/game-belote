using System.Linq;
using Belote.Domain;
using Belote.Game;
using Xunit;
using static AssertNet.Assertions;

namespace Belote.Tests.Game;

public class GameTests
{
    [Fact]
    public void ScoringPile_onPlainSuitContract_RoundsDownForWinner()
    {
        var deck = Belote.Game.Game.GetPlayingDeck();

        deck.Remove(Card.Diamonds_9);
        deck.Remove(Card.Spades_Q);
        deck.Remove(Card.Diamonds_J);
        deck.Remove(Card.Diamonds_10);

        var contract = Contract.Hearts;


        //FIXME: test Game.CountScore
        //FIXME: test Game.MatchScoreToGameScore
        var score = deck.Sum(c => c.Value(contract));

        AssertThat(score).IsEqualTo(156);
        //TODO: game score should be 15 , but naive rounding gives 16
    }
}