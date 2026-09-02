using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp.Models
{
    /// <summary>
    /// Hesap üzerindeki para hareketlerinin yönünü ve türünü belirler.
    /// </summary>
    public enum TransactionType
    {
        Deposit = 1,       // Hesaba para yatırma
        Withdraw = 2,      // Hesaptan nakit para çekme
        TransferOut = 3,   // Başka bir hesaba giden transfer
        TransferIn = 4     // Başka bir hesaptan gelen transfer
    }
}
