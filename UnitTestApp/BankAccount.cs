using System;
using System.Collections.Generic;
using UnitTestApp.Models;

namespace UnitTestApp
{
    /// <summary>
    /// Hesap bakiyesini ve o hesaba ait işlem geçmişini yöneten sınıf.
    /// </summary>
    public class BankAccount
    {
        // Koleksiyonun dışarıdan manipüle edilmesini engellemek için private tutulur
        private readonly List<Transaction> _transactions = new();

        public string AccountHolder { get; }
        public decimal Balance { get; private set; }

        // Dış dünyaya sadece okunabilir (IReadOnlyCollection) liste sunulur
        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public BankAccount(string accountHolder, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountHolder))
                throw new ArgumentException("Hesap sahibi adı boş olamaz.", nameof(accountHolder));

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Başlangıç bakiyesi negatif olamaz.");

            AccountHolder = accountHolder;
            Balance = initialBalance;

            // Başlangıç tutarı varsa ilk hareket (açılış bakiyesi) olarak kaydedilir
            if (initialBalance > 0)
            {
                _transactions.Add(new Transaction(TransactionType.Deposit, initialBalance, "Açılış bakiyesi"));
            }
        }

        // Para Yatırma
        public void Deposit(decimal amount, string description = "Nakit Para Yatırma")
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Yatırılacak tutar 0'dan büyük olmalıdır.");

            Balance += amount;
            _transactions.Add(new Transaction(TransactionType.Deposit, amount, description));
        }

        // Para Çekme
        public void Withdraw(decimal amount, string description = "Nakit Para Çekme")
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Çekilecek tutar 0'dan büyük olmalıdır.");

            if (amount > Balance)
                throw new InvalidOperationException("Yetersiz bakiye.");

            Balance -= amount;
            _transactions.Add(new Transaction(TransactionType.Withdraw, amount, description));
        }

        // Transfer Çıkışı (internal: Sadece TransferService gibi aynı proje içindeki sınıflar erişebilir)
        internal void TransferOut(decimal amount, string targetAccountHolder)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Transfer tutarı 0'dan büyük olmalıdır.");

            if (amount > Balance)
                throw new InvalidOperationException("Transfer için yetersiz bakiye.");

            Balance -= amount;
            _transactions.Add(new Transaction(TransactionType.TransferOut, amount, $"{targetAccountHolder} alıcısına transfer."));
        }

        // Transfer Girişi
        internal void TransferIn(decimal amount, string sourceAccountHolder)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Transfer tutarı 0'dan büyük olmalıdır.");

            Balance += amount;
            _transactions.Add(new Transaction(TransactionType.TransferIn, amount, $"{sourceAccountHolder} kaynağından transfer."));
        }
    }
}