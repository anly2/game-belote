using System.Collections.Generic;
using System.Linq;
using Belote.Domain;
using Belote.Game;
using Belote.Player;
using Xunit;
using static AssertNet.Assertions;
using static Belote.Game.Game;

namespace Belote.Tests.Game;

public class GameTests
{
    [Fact]
    public void SumOfPointsFromCardsIsCorrectForEachContract()
    {
        var deck = GetPlayingDeck();

        AssertThat(deck.Sum(c => c.Value(Contract.Clubs))).IsEqualTo(152);
        AssertThat(deck.Sum(c => c.Value(Contract.Diamonds))).IsEqualTo(152);
        AssertThat(deck.Sum(c => c.Value(Contract.Hearts))).IsEqualTo(152);
        AssertThat(deck.Sum(c => c.Value(Contract.Spades))).IsEqualTo(152);
        AssertThat(deck.Sum(c => c.Value(Contract.NoTrumps))).IsEqualTo(120);
        AssertThat(deck.Sum(c => c.Value(Contract.AllTrumps))).IsEqualTo(248);
    }

    [Fact]
    public void ConvertingMatchScoreToGameScore_AccountsForSpecialRoundingLimits_WithBothTeamsOnTheLimit_OnSuitContracts()
    {
        var points = MatchScoreToGameScore(new[] {36, 126}, Contract.Diamonds);
        AssertThat(points).ContainsExactly(4, 12);
    }

    [Fact] public void ConvertingMatchScoreToGameScore_AccountsForSpecialRoundingLimits_WithBothTeamsOnTheLimit_OnAllTrumpsContract()
    {
        var points = MatchScoreToGameScore(new[] {154, 144}, Contract.AllTrumps);
        AssertThat(points).ContainsExactly(15, 15);
    }

    [Fact]
    public void ConvertingMatchScoreToGameScore_AccountsForSpecialRoundingLimits_OnSuitContracts()
    {
        var points = MatchScoreToGameScore(new[] {35, 127}, Contract.Diamonds);
        AssertThat(points).ContainsExactly(3, 13);
    }

    [Fact]
    public void ConvertingMatchScoreToGameScore_AccountsForSpecialRoundingLimits_OnNoTrumpsContract()
    {
        var points = MatchScoreToGameScore(new[] {34, 225}, Contract.NoTrumps);
        AssertThat(points).ContainsExactly(3, 23);
    }

    [Fact] public void ConvertingMatchScoreToGameScore_AccountsForSpecialRoundingLimits_OnAllTrumpsContract()
    {
        var points = MatchScoreToGameScore(new[] {54, 205}, Contract.AllTrumps);
        AssertThat(points).ContainsExactly(6, 21);
    }
}