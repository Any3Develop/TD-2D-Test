using System.Collections.Generic;

namespace Identity
{
    public class UserIdentity
    {
        public Dictionary<string, Wallet> Wallets { get; private set; } = new();
    }
}