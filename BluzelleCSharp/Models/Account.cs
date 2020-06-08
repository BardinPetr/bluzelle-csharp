using System.Collections.Generic;

namespace BluzelleCSharp.Models
{
    public class Account
    {
        public string Type;
        public AccountData Value;

        public class Coin
        {
            public ulong Amount { get; set; }
            public string Denom { get; set; }
        }

        public class AccountData
        {
            public string Address { get; set; }
            public string PublicKey { get; set; }
            public int AccountNumber { get; set; }
            public int Sequence { get; set; }
            public List<Coin> Coins { get; set; }
        }
    }
}