using Belote.Domain;
using Xunit;
using static AssertNet.Assertions;

namespace TestBelote;

public class ContractTest
{
    [Fact]
    public void ContractsAreOrdered()
    {
        AssertThat(Contract.Clubs < Contract.Diamonds).IsTrue();
        AssertThat(Contract.Diamonds < Contract.Hearts).IsTrue();
        AssertThat(Contract.Hearts < Contract.Spades).IsTrue();

        AssertThat(Contract.Clubs < Contract.Hearts).IsTrue();
        AssertThat(Contract.Clubs < Contract.Spades).IsTrue();
        AssertThat(Contract.Diamonds < Contract.Spades).IsTrue();
    }

    [Fact]
    public void ContractsDoubledAreOrdered()
    {
        AssertThat(Contract.Clubs < Contract.Clubs_Contre).IsTrue();
        AssertThat(Contract.Clubs < Contract.Clubs_Recontra).IsTrue();
        AssertThat(Contract.Clubs_Contre < Contract.Clubs_Recontra).IsTrue();
        AssertThat(Contract.Clubs_Recontra < Contract.Diamonds).IsTrue();
    }

    [Fact]
    public void ContractsCanBeDerived()
    {
        AssertThat(Contract.Clubs.Plain()).IsEqualTo(Contract.Clubs);
        AssertThat(Contract.Clubs.Contre()).IsEqualTo(Contract.Clubs_Contre);
        AssertThat(Contract.Clubs.Recontra()).IsEqualTo(Contract.Clubs_Recontra);
        AssertThat(Contract.Clubs_Contre.Plain()).IsEqualTo(Contract.Clubs);
        AssertThat(Contract.Clubs_Contre.Contre()).IsEqualTo(Contract.Clubs_Contre);
        AssertThat(Contract.Clubs_Contre.Recontra()).IsEqualTo(Contract.Clubs_Recontra);
        AssertThat(Contract.Clubs_Recontra.Plain()).IsEqualTo(Contract.Clubs);
        AssertThat(Contract.Clubs_Recontra.Contre()).IsEqualTo(Contract.Clubs_Contre);
        AssertThat(Contract.Clubs_Recontra.Recontra()).IsEqualTo(Contract.Clubs_Recontra);

        AssertThat(Contract.Diamonds.Plain()).IsEqualTo(Contract.Diamonds);
        AssertThat(Contract.Diamonds.Contre()).IsEqualTo(Contract.Diamonds_Contre);
        AssertThat(Contract.Diamonds.Recontra()).IsEqualTo(Contract.Diamonds_Recontra);
        AssertThat(Contract.Diamonds_Contre.Plain()).IsEqualTo(Contract.Diamonds);
        AssertThat(Contract.Diamonds_Contre.Contre()).IsEqualTo(Contract.Diamonds_Contre);
        AssertThat(Contract.Diamonds_Contre.Recontra()).IsEqualTo(Contract.Diamonds_Recontra);
        AssertThat(Contract.Diamonds_Recontra.Plain()).IsEqualTo(Contract.Diamonds);
        AssertThat(Contract.Diamonds_Recontra.Contre()).IsEqualTo(Contract.Diamonds_Contre);
        AssertThat(Contract.Diamonds_Recontra.Recontra()).IsEqualTo(Contract.Diamonds_Recontra);

        AssertThat(Contract.Hearts.Plain()).IsEqualTo(Contract.Hearts);
        AssertThat(Contract.Hearts.Contre()).IsEqualTo(Contract.Hearts_Contre);
        AssertThat(Contract.Hearts.Recontra()).IsEqualTo(Contract.Hearts_Recontra);
        AssertThat(Contract.Hearts_Contre.Plain()).IsEqualTo(Contract.Hearts);
        AssertThat(Contract.Hearts_Contre.Contre()).IsEqualTo(Contract.Hearts_Contre);
        AssertThat(Contract.Hearts_Contre.Recontra()).IsEqualTo(Contract.Hearts_Recontra);
        AssertThat(Contract.Hearts_Recontra.Plain()).IsEqualTo(Contract.Hearts);
        AssertThat(Contract.Hearts_Recontra.Contre()).IsEqualTo(Contract.Hearts_Contre);
        AssertThat(Contract.Hearts_Recontra.Recontra()).IsEqualTo(Contract.Hearts_Recontra);

        AssertThat(Contract.Spades.Plain()).IsEqualTo(Contract.Spades);
        AssertThat(Contract.Spades.Contre()).IsEqualTo(Contract.Spades_Contre);
        AssertThat(Contract.Spades.Recontra()).IsEqualTo(Contract.Spades_Recontra);
        AssertThat(Contract.Spades_Contre.Plain()).IsEqualTo(Contract.Spades);
        AssertThat(Contract.Spades_Contre.Contre()).IsEqualTo(Contract.Spades_Contre);
        AssertThat(Contract.Spades_Contre.Recontra()).IsEqualTo(Contract.Spades_Recontra);
        AssertThat(Contract.Spades_Recontra.Plain()).IsEqualTo(Contract.Spades);
        AssertThat(Contract.Spades_Recontra.Contre()).IsEqualTo(Contract.Spades_Contre);
        AssertThat(Contract.Spades_Recontra.Recontra()).IsEqualTo(Contract.Spades_Recontra);
    }
}