using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Moq;
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
        // ----------------- TEMEL ÝÞLEM TESTLERÝ -----------------

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
            Assert.Equal(100, account.Balance);
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

        // ----------------- TRANSFER VE LOG DOÐRULAMA TESTLERÝ -----------------

        [Fact]
        public void Transfer_YeterliBakiyeIle_BasariliDonmeliVeInformationLoguUretmeli()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TransferService>>();
            var service = new TransferService(loggerMock.Object);

            var gonderen = new BankAccount("Ahmet", 500);
            var alici = new BankAccount("Mehmet", 100);

            // Act
            var result = service.Transfer(gonderen, alici, 200);

            // Assert: Durum ve bakiye doðrulamasý
            Assert.True(result.IsSuccess);
            Assert.Equal(300, gonderen.Balance);
            Assert.Equal(300, alici.Balance);

            // Assert: Log doðrulama (LogInformation seviyesinde en az bir kez çaðrýldý mý?)
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public void Transfer_YetersizBakiyeDurumunda_InsufficientFundsDonmeliVeWarningLoguUretmeli()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TransferService>>();
            var service = new TransferService(loggerMock.Object);

            var gonderen = new BankAccount("Ahmet", 100);
            var alici = new BankAccount("Mehmet", 200);

            // Act
            var result = service.Transfer(gonderen, alici, 500);

            // Assert: Sonuç doðrulamasý
            Assert.True(result.IsFailure);
            Assert.Equal(BankErrors.InsufficientFunds.Code, result.Error.Code);
            Assert.Equal(100, gonderen.Balance);
            Assert.Equal(200, alici.Balance);

            // Assert: Log doðrulama (LogWarning seviyesinde çaðrýldý mý?)
            loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public void Transfer_AyniHesabaGonderildiginde_SameAccountHatasiDonmeli()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<TransferService>>();
            var service = new TransferService(loggerMock.Object);

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