using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestApp.Common
{
    /// <summary>
    /// Exception fırlatmadan işlem sonucunu (başarılı/başarısız) taşıyan sonuç sınıfı.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public Error Error { get; }

        protected Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
                throw new InvalidOperationException("Başarılı bir sonuç hata içeremez.");

            if (!isSuccess && error == Error.None)
                throw new InvalidOperationException("Başarısız bir sonuç geçerli bir hata içermelidir.");

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
    }
}
