using Microsoft.Extensions.Logging;
using UnitTestApp.Common;
using UnitTestApp.Errors;

namespace UnitTestApp.Services
{
    public class TransferService
    {
        private readonly ILogger<TransferService> _logger;

        // Constructor Injection ile ILogger arayüzü alınıyor
        public TransferService(ILogger<TransferService> logger)
        {
            _logger = logger;
        }

        public Result Transfer(BankAccount source, BankAccount target, decimal amount)
        {
            // Null kontrolleri
            if (source == null || target == null)
            {
                _logger.LogWarning("Transfer basarisiz: Hesap bilgisi eksik (null).");
                return Result.Failure(BankErrors.NullAccount);
            }

            // Aynı hesap kontrolü
            if (ReferenceEquals(source, target))
            {
                _logger.LogWarning("Transfer basarisiz: {AccountHolder} adina ayni hesaba aktarim yapilamaz.", source.AccountHolder);
                return Result.Failure(BankErrors.SameAccountTransfer);
            }

            // Tutar kontrolü
            if (amount <= 0)
            {
                _logger.LogWarning("Transfer basarisiz: Gecersiz transfer tutari ({Amount}).", amount);
                return Result.Failure(BankErrors.InvalidAmount);
            }

            // Kaynaktan çekim dene
            var withdrawResult = source.TransferOut(amount, target.AccountHolder);
            if (withdrawResult.IsFailure)
            {
                _logger.LogWarning(
                    "Transfer basarisiz: {SourceHolder} hesabinda yetersiz bakiye. Istenen Tutar: {Amount}, Mevcut Bakiye: {Balance}",
                    source.AccountHolder,
                    amount,
                    source.Balance);

                return withdrawResult;
            }

            // Alıcıya aktar
            target.TransferIn(amount, source.AccountHolder);

            _logger.LogInformation(
                "Transfer basarili: {SourceHolder} -> {TargetHolder}, Tutar: {Amount}",
                source.AccountHolder,
                target.AccountHolder,
                amount);

            return Result.Success();
        }
    }
}