using System;
using System.Collections.Generic;
using UnitTestApp.Common;
using UnitTestApp.Errors;
using UnitTestApp.Models;

namespace UnitTestApp
{
    public class BankAccount
    {
        private readonly List<Transaction> _transactions = new();

        public string AccountHolder { get; }
        public decimal Balance { get; private set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        // Constructor'da doğrudan nesne oluşturulamama durumu için guard clause kalır
        public BankAccount(string accountHolder, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountHolder))
                throw new ArgumentException(BankErrors.EmptyAccountHolder.Description, nameof(accountHolder));

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), BankErrors.NegativeInitialBalance.Description);

            AccountHolder = accountHolder;
            Balance = initialBalance;

            if (initialBalance > 0)
            {
                _transactions.Add(new Transaction(TransactionType.Deposit, initialBalance, "Açılış bakiyesi"));
            }
        }

        // Para Yatırma: Hata durumunda exception yerine Result döner
        public Result Deposit(decimal amount, string description = "Nakit Para Yatırma")
        {
            if (amount <= 0)
                return Result.Failure(BankErrors.InvalidAmount);

            Balance += amount;
            _transactions.Add(new Transaction(TransactionType.Deposit, amount, description));

            return Result.Success();
        }

        // Para Çekme: Yetersiz bakiye durumunda Result.Failure döner
        public Result Withdraw(decimal amount, string description = "Nakit Para Çekme")
        {
            if (amount <= 0)
                return Result.Failure(BankErrors.InvalidAmount);

            if (amount > Balance)
                return Result.Failure(BankErrors.InsufficientFunds);

            Balance -= amount;
            _transactions.Add(new Transaction(TransactionType.Withdraw, amount, description));

            return Result.Success();
        }

        internal Result TransferOut(decimal amount, string targetAccountHolder)
        {
            if (amount <= 0)
                return Result.Failure(BankErrors.InvalidAmount);

            if (amount > Balance)
                return Result.Failure(BankErrors.InsufficientFunds);

            Balance -= amount;
            _transactions.Add(new Transaction(TransactionType.TransferOut, amount, $"{targetAccountHolder} alıcısına transfer."));

            return Result.Success();
        }

        internal Result TransferIn(decimal amount, string sourceAccountHolder)
        {
            if (amount <= 0)
                return Result.Failure(BankErrors.InvalidAmount);

            Balance += amount;
            _transactions.Add(new Transaction(TransactionType.TransferIn, amount, $"{sourceAccountHolder} kaynağından transfer."));

            return Result.Success();
        }
    }
}