using UnitTestApp;
using Xunit;

namespace UnitTestApp.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void Deposit_PozitifTutarGirildiginde_BakiyeArtmali()
        {
            // 1. Arrange (Hazýrlýk)
            var account = new BankAccount("Ahmet Yýlmaz", 100);
            decimal depositAmount = 50;

            // 2. Act (Eylem)
            account.Deposit(depositAmount);

            // 3. Assert (Doðrulama)
            Assert.Equal(150, account.Balance);
        }
        [Fact]
        public void Deposit_SifirVeyaNegatifTutarGirildiginde_ArgumentOutOfRangeExceptionFirlatmali()
        {
            // Arrange (Hazýrlýk)
            var account = new BankAccount("Ahmet Yýlmaz", 100);

            // Act & Assert (Eylem ve Doðrulama)
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Deposit(-50));
        }
        // TEST 3: Normal Para Çekme
        [Fact]
        public void Withdraw_YeterliBakiyeOldugunda_BakiyeDusmeli()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 200);
            decimal withdrawAmount = 75;

            // Act
            account.Withdraw(withdrawAmount);

            // Assert
            Assert.Equal(125, account.Balance);
        }

        // TEST 4: Yetersiz Bakiye ve Hata Mesajý Kontrolü
        [Fact]
        public void Withdraw_BakiyeYetersizOldugunda_InvalidOperationExceptionFirlatmali()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 50);

            // Act & Assert (Fýrlatýlan hatayý bir deðiþkene yakalýyoruz)
            var exception = Assert.Throws<InvalidOperationException>(() => account.Withdraw(100));

            // Hata türüne ek olarak fýrlatýlan mesajýn doðruluðunu da test ediyoruz
            Assert.Equal("Yetersiz bakiye.", exception.Message);
        }
    }
}