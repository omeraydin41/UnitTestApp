using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp.Common
{
    /// <summary>
    /// Hatanın operasyonel seviyedeki niteliğini belirler.
    /// HTTP durum kodlarına veya loglama seviyelerine eşleme yapılmasını kolaylaştırır.
    /// </summary>
    public enum ErrorType
    {
        Failure = 1,      // Genel operasyonel hata
        Validation = 2,   // İş kuralı veya girdi doğrulama hatası
        NotFound = 3,     // İlgili kayıt/hesap bulunamadı
        Conflict = 4      // Durum çakışması
    }
}
