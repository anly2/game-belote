using Belote.Domain;
using Xunit;
using static AssertNet.Assertions;

namespace TestBelote;

public class ContractTest
{
    [Fact]
    public void ContractsAreOrdered()
    {
        AssertThat(Contract.Clubs < Contract.Diamond).IsTrue();
        AssertThat(Contract.Diamond < Contract.Hearts).IsTrue();
        AssertThat(Contract.Hearts < Contract.Spades).IsTrue();

        AssertThat(Contract.Clubs < Contract.Hearts).IsTrue();
        AssertThat(Contract.Clubs < Contract.Spades).IsTrue();
        AssertThat(Contract.Diamond < Contract.Spades).IsTrue();
    }

    [Fact]
    public void ContractsDoubledAreOrdered()
    {
        AssertThat(Contract.Clubs < Contract.Clubs_Contre).IsTrue();
        AssertThat(Contract.Clubs < Contract.Clubs_Recontra).IsTrue();
        AssertThat(Contract.Clubs_Contre < Contract.Clubs_Recontra).IsTrue();
        AssertThat(Contract.Clubs_Recontra < Contract.Diamond).IsTrue();
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

        AssertThat(Contract.Diamond.Plain()).IsEqualTo(Contract.Diamond);
        AssertThat(Contract.Diamond.Contre()).IsEqualTo(Contract.Diamond_Contre);
        AssertThat(Contract.Diamond.Recontra()).IsEqualTo(Contract.Diamond_Recontra);
        AssertThat(Contract.Diamond_Contre.Plain()).IsEqualTo(Contract.Diamond);
        AssertThat(Contract.Diamond_Contre.Contre()).IsEqualTo(Contract.Diamond_Contre);
        AssertThat(Contract.Diamond_Contre.Recontra()).IsEqualTo(Contract.Diamond_Recontra);
        AssertThat(Contract.Diamond_Recontra.Plain()).IsEqualTo(Contract.Diamond);
        AssertThat(Contract.Diamond_Recontra.Contre()).IsEqualTo(Contract.Diamond_Contre);
        AssertThat(Contract.Diamond_Recontra.Recontra()).IsEqualTo(Contract.Diamond_Recontra);

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