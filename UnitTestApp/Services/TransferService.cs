using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp.Services
{

    /// <summary>
    /// İki BankAccount nesnesi arasındaki para aktarımını (virman) yöneten servis.
    /// </summary>
    public class TransferService
    {
        public void Transfer(BankAccount source, BankAccount target, decimal amount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source), "Kaynak hesap boş olamaz.");

            if (target == null)
                throw new ArgumentNullException(nameof(target), "Hedef hesap boş olamaz.");

            if (ReferenceEquals(source, target))
                throw new InvalidOperationException("Aynı hesaba transfer yapılamaz.");

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Transfer tutarı 0'dan büyük olmalıdır.");

            // 1. Gönderen hesaptan çekilir (bakiye yetersizse exception fırlatır, hedef hesaba geçmez)
            source.TransferOut(amount, target.AccountHolder);

            // 2. Alıcı hesaba eklenir
            target.TransferIn(amount, source.AccountHolder);
        }
    }
}
