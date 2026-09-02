using System.Linq;
using UnitTestApp;
using UnitTestApp.Common;
using UnitTestApp.Errors;
using UnitTestApp.Models;
using UnitTestApp.Services;
using Xunit;

namespace UnitTestApp.Tests
{
    public class BankAccountTests
    {
        [Fact]
        public void Deposit_PozitifTutarGirildiginde_BasariliDonmeliVeBakiyeArtmali()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 100);

            // Act
            var result = account.Deposit(50);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(Error.None, result.Error);
            Assert.Equal(150, account.Balance);
        }

        [Fact]
        public void Deposit_SifirVeyaNegatifTutarGirildiginde_InvalidAmountHatasiDonmeli()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 100);

            // Act
            var result = account.Deposit(-50);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(BankErrors.InvalidAmount.Code, result.Error.Code);
            Assert.Equal(100, account.Balance); // Bakiye deðiþmemeli
        }

        [Fact]
        public void Withdraw_YetersizBakiyeOldugunda_InsufficientFundsHatasiDonmeli()
        {
            // Arrange
            var account = new BankAccount("Ahmet Yýlmaz", 50);

            // Act
            var result = account.Withdraw(100);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(BankErrors.InsufficientFunds.Code, result.Error.Code);
            Assert.Equal(ErrorType.Failure, result.Error.Type);
            Assert.Equal(50, account.Balance);
        }

        [Fact]
        public void Transfer_YeterliBakiyeIle_BasariliDonmeliVeBakiyelerGuncellenmeli()
        {
            // Arrange
            var service = new TransferService();
            var gonderen = new BankAccount("Ahmet", 500);
            var alici = new BankAccount("Mehmet", 100);

            // Act
            var result = service.Transfer(gonderen, alici, 200);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(300, gonderen.Balance);
            Assert.Equal(300, alici.Balance);
        }

        [Fact]
        public void Transfer_YetersizBakiyeDurumunda_InsufficientFundsDonmeliVeBakiyelerDegismemeli()
        {
            // Arrange
            var service = new TransferService();
            var gonderen = new BankAccount("Ahmet", 100);
            var alici = new BankAccount("Mehmet", 200);

            // Act
            var result = service.Transfer(gonderen, alici, 500);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(BankErrors.InsufficientFunds.Code, result.Error.Code);
            Assert.Equal(100, gonderen.Balance);
            Assert.Equal(200, alici.Balance);
        }

        [Fact]
        public void Transfer_AyniHesabaGonderildiginde_SameAccountHatasiDonmeli()
        {
            // Arrange
            var service = new TransferService();
            var hesap = new BankAccount("Ahmet", 500);

            // Act
            var result = service.Transfer(hesap, hesap, 100);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(BankErrors.SameAccountTransfer.Code, result.Error.Code);
            Assert.Equal(ErrorType.Conflict, result.Error.Type);
        }
    }
}