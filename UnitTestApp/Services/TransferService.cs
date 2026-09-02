using UnitTestApp.Common;
using UnitTestApp.Errors;

namespace UnitTestApp.Services
{
    public class TransferService
    {
        public Result Transfer(BankAccount source, BankAccount target, decimal amount)
        {
            // Null kontrolleri
            if (source == null || target == null)
                return Result.Failure(BankErrors.NullAccount);

            // Aynı hesap kontrolü
            if (ReferenceEquals(source, target))
                return Result.Failure(BankErrors.SameAccountTransfer);

            // Tutar kontrolü
            if (amount <= 0)
                return Result.Failure(BankErrors.InvalidAmount);

            // Kaynaktan çekim dene: Başarısız olursa doğrudan dön ve hedef hesaba dokunma
            var withdrawResult = source.TransferOut(amount, target.AccountHolder);
            if (withdrawResult.IsFailure)
                return withdrawResult;

            // Alıcıya aktar
            return target.TransferIn(amount, source.AccountHolder);
        }
    }
}