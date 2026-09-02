using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp.Models
{
    /// <summary>
 /// Bir banka hesabında gerçekleşen tekil finansal hareketi (dekontu) temsil eder.
 /// Bilgilerin sonradan değiştirilememesi için property'ler getter-only yapılmıştır.
 /// </summary>
    public class Transaction
    {
        public Guid Id { get; }
        public TransactionType Type { get; }
        public decimal Amount { get; }
        public DateTime Timestamp { get; }
        public string Description { get; }

        public Transaction(TransactionType type, decimal amount, string description = "")
        {
            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "İşlem tutarı 0'dan büyük olmalıdır.");

            Id = Guid.NewGuid();
            Type = type;
            Amount = amount;
            Timestamp = DateTime.UtcNow;
            Description = description;
        }
    }
}
