using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp.Common
{
    /// <summary>
    /// Domain genelinde kullanılan değişmez (immutable) hata nesnesi.
    /// String bazlı rastgele hata mesajları yerine tip güvenli kod ve tip taşır.
    /// </summary>
    public sealed record Error(string Code, string Description, ErrorType Type)
    {
        // Başarılı durumlarda dönülecek nötr hata temsili
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        public static Error Validation(string code, string description) =>
            new(code, description, ErrorType.Validation);

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);
    }
}
