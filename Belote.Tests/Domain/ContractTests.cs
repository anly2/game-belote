using AssertNet;
using Belote.Domain;
using Xunit;

namespace Belote.Tests.Domain;

public class ContractTests
{
    [Fact]
    public void ContractsAreOrdered()
    {
        Assertions.AssertThat(Contract.Clubs < Contract.Diamonds).IsTrue();
        Assertions.AssertThat(Contract.Diamonds < Contract.Hearts).IsTrue();
        Assertions.AssertThat(Contract.Hearts < Contract.Spades).IsTrue();

        Assertions.AssertThat(Contract.Clubs < Contract.Hearts).IsTrue();
        Assertions.AssertThat(Contract.Clubs < Contract.Spades).IsTrue();
        Assertions.AssertThat(Contract.Diamonds < Contract.Spades).IsTrue();
    }

    [Fact]
    public void ContractsDoubledAreOrdered()
    {
        Assertions.AssertThat(Contract.Clubs < Contract.Clubs_Contre).IsTrue();
        Assertions.AssertThat(Contract.Clubs < Contract.Clubs_Recontra).IsTrue();
        Assertions.AssertThat(Contract.Clubs_Contre < Contract.Clubs_Recontra).IsTrue();
        Assertions.AssertThat(Contract.Clubs_Recontra < Contract.Diamonds).IsTrue();
    }

    [Fact]
    public void ContractsCanBeDerived()
    {
        Assertions.AssertThat(Contract.Clubs.Plain()).IsEqualTo(Contract.Clubs);
        Assertions.AssertThat(Contract.Clubs.Contre()).IsEqualTo(Contract.Clubs_Contre);
        Assertions.AssertThat(Contract.Clubs.Recontra()).IsEqualTo(Contract.Clubs_Recontra);
        Assertions.AssertThat(Contract.Clubs_Contre.Plain()).IsEqualTo(Contract.Clubs);
        Assertions.AssertThat(Contract.Clubs_Contre.Contre()).IsEqualTo(Contract.Clubs_Contre);
        Assertions.AssertThat(Contract.Clubs_Contre.Recontra()).IsEqualTo(Contract.Clubs_Recontra);
        Assertions.AssertThat(Contract.Clubs_Recontra.Plain()).IsEqualTo(Contract.Clubs);
        Assertions.AssertThat(Contract.Clubs_Recontra.Contre()).IsEqualTo(Contract.Clubs_Contre);
        Assertions.AssertThat(Contract.Clubs_Recontra.Recontra()).IsEqualTo(Contract.Clubs_Recontra);

        Assertions.AssertThat(Contract.Diamonds.Plain()).IsEqualTo(Contract.Diamonds);
        Assertions.AssertThat(Contract.Diamonds.Contre()).IsEqualTo(Contract.Diamonds_Contre);
        Assertions.AssertThat(Contract.Diamonds.Recontra()).IsEqualTo(Contract.Diamonds_Recontra);
        Assertions.AssertThat(Contract.Diamonds_Contre.Plain()).IsEqualTo(Contract.Diamonds);
        Assertions.AssertThat(Contract.Diamonds_Contre.Contre()).IsEqualTo(Contract.Diamonds_Contre);
        Assertions.AssertThat(Contract.Diamonds_Contre.Recontra()).IsEqualTo(Contract.Diamonds_Recontra);
        Assertions.AssertThat(Contract.Diamonds_Recontra.Plain()).IsEqualTo(Contract.Diamonds);
        Assertions.AssertThat(Contract.Diamonds_Recontra.Contre()).IsEqualTo(Contract.Diamonds_Contre);
        Assertions.AssertThat(Contract.Diamonds_Recontra.Recontra()).IsEqualTo(Contract.Diamonds_Recontra);

        Assertions.AssertThat(Contract.Hearts.Plain()).IsEqualTo(Contract.Hearts);
        Assertions.AssertThat(Contract.Hearts.Contre()).IsEqualTo(Contract.Hearts_Contre);
        Assertions.AssertThat(Contract.Hearts.Recontra()).IsEqualTo(Contract.Hearts_Recontra);
        Assertions.AssertThat(Contract.Hearts_Contre.Plain()).IsEqualTo(Contract.Hearts);
        Assertions.AssertThat(Contract.Hearts_Contre.Contre()).IsEqualTo(Contract.Hearts_Contre);
        Assertions.AssertThat(Contract.Hearts_Contre.Recontra()).IsEqualTo(Contract.Hearts_Recontra);
        Assertions.AssertThat(Contract.Hearts_Recontra.Plain()).IsEqualTo(Contract.Hearts);
        Assertions.AssertThat(Contract.Hearts_Recontra.Contre()).IsEqualTo(Contract.Hearts_Contre);
        Assertions.AssertThat(Contract.Hearts_Recontra.Recontra()).IsEqualTo(Contract.Hearts_Recontra);

        Assertions.AssertThat(Contract.Spades.Plain()).IsEqualTo(Contract.Spades);
        Assertions.AssertThat(Contract.Spades.Contre()).IsEqualTo(Contract.Spades_Contre);
        Assertions.AssertThat(Contract.Spades.Recontra()).IsEqualTo(Contract.Spades_Recontra);
        Assertions.AssertThat(Contract.Spades_Contre.Plain()).IsEqualTo(Contract.Spades);
        Assertions.AssertThat(Contract.Spades_Contre.Contre()).IsEqualTo(Contract.Spades_Contre);
        Assertions.AssertThat(Contract.Spades_Contre.Recontra()).IsEqualTo(Contract.Spades_Recontra);
        Assertions.AssertThat(Contract.Spades_Recontra.Plain()).IsEqualTo(Contract.Spades);
        Assertions.AssertThat(Contract.Spades_Recontra.Contre()).IsEqualTo(Contract.Spades_Contre);
        Assertions.AssertThat(Contract.Spades_Recontra.Recontra()).IsEqualTo(Contract.Spades_Recontra);
    }
}