using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp
{
    public class BankAccount
    {
        public string AccountHolder { get; }
        public decimal Balance { get; private set; }

        public BankAccount(string accountHolder, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountHolder))
                throw new ArgumentException("Hesap sahibi adı boş olamaz.", nameof(accountHolder));

            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Başlangıç bakiyesi negatif olamaz.");

            AccountHolder = accountHolder;
            Balance = initialBalance;
        }

        // Para Yatırma
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Yatırılacak tutar 0'dan büyük olmalıdır.");

            Balance += amount;
        }

        // Para Çekme
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Çekilecek tutar 0'dan büyük olmalıdır.");

            if (amount > Balance)
                throw new InvalidOperationException("Yetersiz bakiye.");

            Balance -= amount;
        }
    }
}
