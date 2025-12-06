using System;

namespace Identity
{
    public class Wallet : IDisposable
    {
        public string Id { get; }
        public int Amount { get; private set; }
        public int Previous { get; private set; }
        
        public event Action<Wallet> OnChange;

        public Wallet(string id)
        {
            Id = id;
        }

        public void Deposit(int amount)
        {
            if (amount < 0)
                throw new InvalidOperationException("Cannot deposit an amount less than 0");
            
            Previous = Amount;
            Amount += amount;
            OnChange?.Invoke(this);
        }

        public bool Withdraw(int amount)
        {
            if (amount > Amount)
                return false;
            
            Previous = Amount;
            Amount -= amount;
            OnChange?.Invoke(this);
            return true;
        }

        public void Dispose()
        {
            OnChange = null;
        }
    }
}