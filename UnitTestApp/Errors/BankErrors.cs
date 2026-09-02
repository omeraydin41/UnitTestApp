using UnitTestApp.Common;

namespace UnitTestApp.Errors
{
    /// <summary>
    /// Bankacılık domain'ine ait merkezi hata sözlüğü.
    /// Tüm hata kodları tek bir yerden yönetilir ve testlerde doğrudan referans alınır.
    /// </summary>
    public static class BankErrors
    {
        public static readonly Error EmptyAccountHolder = Error.Validation(
            "Account.EmptyHolder",
            "Hesap sahibi adı boş veya geçersiz olamaz.");

        public static readonly Error NegativeInitialBalance = Error.Validation(
            "Account.NegativeInitialBalance",
            "Başlangıç bakiyesi negatif olamaz.");

        public static readonly Error InvalidAmount = Error.Validation(
            "Account.InvalidAmount",
            "İşlem tutarı 0'dan büyük olmalıdır.");

        public static readonly Error InsufficientFunds = Error.Failure(
            "Account.InsufficientFunds",
            "Hesapta bu işlem için yeterli bakiye bulunmamaktadır.");

        public static readonly Error SameAccountTransfer = Error.Conflict(
            "Transfer.SameAccount",
            "Kaynak ve hedef hesap aynı olamaz.");

        public static readonly Error NullAccount = Error.Validation(
            "Transfer.NullAccount",
            "İşlem yapılacak hesap bilgisi boş (null) olamaz.");
    }
}