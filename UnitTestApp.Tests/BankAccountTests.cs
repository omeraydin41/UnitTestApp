using System;
using System.Linq;
using UnitTestApp;
using UnitTestApp.Models;
using UnitTestApp.Services;
using Xunit;

namespace UnitTestApp.Tests
{
    public class BankAccountTests
    {
        // ----------------- TEMEL ÝÞLEM TESTLERÝ -----------------

        [Fact]
        public void Deposit_PozitifTutarGirildiginde_BakiyeArtmaliVeLoglanmali()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 100);

            // Act
            account.Deposit(50, "Maaþ");

            // Assert
            Assert.Equal(150, account.Balance);
            Assert.Equal(2, account.Transactions.Count); // Açýlýþ (1) + Deposit (1)
            Assert.Equal(TransactionType.Deposit, account.Transactions.Last().Type);
        }

        [Fact]
        public void Deposit_SifirVeyaNegatifTutarGirildiginde_ArgumentOutOfRangeExceptionFirlatmali()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 100);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => account.Deposit(-50));
        }

        [Fact]
        public void Withdraw_YeterliBakiyeOldugunda_BakiyeDusmeliVeLoglanmali()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 200);

            // Act
            account.Withdraw(75);

            // Assert
            Assert.Equal(125, account.Balance);
            Assert.Equal(TransactionType.Withdraw, account.Transactions.Last().Type);
        }

        [Fact]
        public void Withdraw_BakiyeYetersizOldugunda_InvalidOperationExceptionFirlatmali()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 50);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => account.Withdraw(100));
            Assert.Equal("Yetersiz bakiye.", ex.Message);
        }

        // ----------------- TRANSFER SERVÝSÝ TESTLERÝ -----------------

        [Fact]
        public void Transfer_YeterliBakiyeIle_HerIkiHesabiGuncellemeli()
        {
            // Arrange
            var service = new TransferService();
            var gonderen = new BankAccount("Ahmet", 1000);
            var alici = new BankAccount("Mehmet", 200);

            // Act
            service.Transfer(gonderen, alici, 300);

            // Assert
            Assert.Equal(700, gonderen.Balance);
            Assert.Equal(500, alici.Balance);

            // Hareket kayýtlarýnýn doðrulanmasý
            Assert.Equal(TransactionType.TransferOut, gonderen.Transactions.Last().Type);
            Assert.Equal(TransactionType.TransferIn, alici.Transactions.Last().Type);
        }

        [Fact]
        public void Transfer_YetersizBakiyede_AliciBakiyesiDegismemeli()
        {
            // Arrange
            var service = new TransferService();
            var gonderen = new BankAccount("Ahmet", 100);
            var alici = new BankAccount("Mehmet", 200);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => service.Transfer(gonderen, alici, 500));
            Assert.Equal("Transfer için yetersiz bakiye.", ex.Message);

            // Hedef hesap etkilenmemeli
            Assert.Equal(100, gonderen.Balance);
            Assert.Equal(200, alici.Balance);
        }

        [Fact]
        public void Transfer_AyniHesapSecildiginde_HataFirlatmali()
        {
            // Arrange
            var service = new TransferService();
            var hesap = new BankAccount("Ahmet", 500);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.Transfer(hesap, hesap, 100));
        }
    }
}