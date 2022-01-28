namespace Belote.Domain
{
    // ReSharper disable InconsistentNaming
    public enum Contract
    {
        Clubs,
        Clubs_Contre,
        Clubs_Recontra,
        Diamonds,
        Diamonds_Contre,
        Diamonds_Recontra,
        Hearts,
        Hearts_Contre,
        Hearts_Recontra,
        Spades,
        Spades_Contre,
        Spades_Recontra,
        NoTrumps,
        NoTrumps_Contre,
        NoTrumps_Recontra,
        AllTrumps,
        AllTrumps_Contre,
        AllTrumps_Recontra
    }

    public static class ContractUtils
    {
        //PlainContracts
        //Text
        //Plain
        //Contre
        //Recontra
        //IsPlain
        //IsContre
        //IsRecontra
        public static readonly Contract[] PlainContracts = {Contract.Clubs, Contract.Diamonds, Contract.Hearts, Contract.Spades, Contract.NoTrumps, Contract.AllTrumps};
        private static readonly string[] ContractTexts =
        {
            "♣", "X♣", "XX♣",
            "♦", "X♦", "XX♦",
            "♥", "X♥", "XX♥",
            "♠", "X♠", "XX♠",
            "A", "XA", "XXA",
            "J", "XJ", "XXJ"
        };

        public static string Text(this Contract contract)
        {
            return ContractTexts[(int) contract];
        }


        public static Contract Plain(this Contract contract)
        {
            return (Contract) ((int) contract - (int) contract % 3);
        }

        public static Contract Contre(this Contract contract)
        {
            return (Contract) ((int) contract.Plain() + 1);
        }
        public static Contract Recontra(this Contract contract)
        {
            return (Contract) ((int) contract.Contre() + 1);
        }

        public static bool IsPlain(this Contract contract)
        {
            return (int) contract % 3 == 0;
        }
        public static bool IsContre(this Contract contract)
        {
            return (int) contract % 3 == 1;
        }
        public static bool IsRecontra(this Contract contract)
        {
            return (int) contract % 3 == 2;
        }
    }
}