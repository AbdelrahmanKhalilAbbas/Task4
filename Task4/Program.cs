using System;
using System.Collections.Generic;

namespace Task4
{
    // Base Account Class
    public class Account
    {
        public string Name { get; set; }
        public decimal Balance { get; protected set; }

        public Account(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
        }

        public virtual bool Deposit(decimal amount)
        {
            if (amount <= 0) return false;
            Balance += amount;
            return true;
        }

        public virtual bool Withdraw(decimal amount)
        {
            if (amount <= 0 || amount > Balance) return false;
            Balance -= amount;
            return true;
        }

        public override string ToString() => $"{Name}: Balance = {Balance:C}";
    }

    // Savings Account Class
    public class SavingsAccount : Account
    {
        public decimal InterestRate { get; set; }

        public SavingsAccount(string name, decimal balance, decimal interestRate)
            : base(name, balance)
        {
            InterestRate = interestRate;
        }

        public override bool Deposit(decimal amount)
        {
            if (base.Deposit(amount))
            {
                Balance += amount * InterestRate / 100;
                return true;
            }
            return false;
        }
    }

    // Checking Account Class
    public class CheckingAccount : Account
    {
        private const decimal WithdrawalFee = 1.50m;

        public CheckingAccount(string name, decimal balance)
            : base(name, balance) { }

        public override bool Withdraw(decimal amount)
        {
            return base.Withdraw(amount + WithdrawalFee);
        }
    }

    // Trust Account Class
    public class TrustAccount : SavingsAccount
    {
        private int withdrawalsThisYear = 0;
        private const int MaxWithdrawalsPerYear = 3;
        private const decimal BonusThreshold = 5000.00m;
        private const decimal BonusAmount = 50.00m;

        public TrustAccount(string name, decimal balance, decimal interestRate)
            : base(name, balance, interestRate) { }

        public override bool Deposit(decimal amount)
        {
            if (base.Deposit(amount))
            {
                if (amount >= BonusThreshold)
                    Balance += BonusAmount;
                return true;
            }
            return false;
        }

        public override bool Withdraw(decimal amount)
        {
            if (withdrawalsThisYear >= MaxWithdrawalsPerYear)
                return false;

            if (amount >= Balance * 0.2m)
                return false;

            if (base.Withdraw(amount))
            {
                withdrawalsThisYear++;
                return true;
            }

            return false;
        }
    }

    // Generic Utility Class
    public static class AccountPrinter
    {
        public static void PrintAccounts<T>(List<T> accounts) where T : Account
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                Console.WriteLine(accounts[i]);
            }
        }
    }

    // Program Entry
    internal class Program
    {
        static void Main(string[] args)
        {
            var accounts = new List<Account>
            {
                new SavingsAccount("Alice", 1000, 5),
                new CheckingAccount("Bob", 1500),
                new TrustAccount("Charlie", 10000, 4)
            };

            for (int i = 0; i < accounts.Count; i++)
            {
                accounts[i].Deposit(5000);
                accounts[i].Withdraw(1000);
            }

            Console.WriteLine("\n--- Account Summaries ---");
            AccountPrinter.PrintAccounts(accounts);
        }
    }
}