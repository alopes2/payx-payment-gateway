using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Moq;
using PayX.Bank.Models;
using PayX.Bank.Services;
using PayX.Core;
using PayX.Core.Exceptions;
using PayX.Core.Models;
using PayX.Service;
using Xunit;

namespace PayX.UnitTests.Services
{
    public class PaymentServiceTests
    {
        private PaymentService _paymentService;

        private Mock<IBankService> _bankService;

        private Mock<IUnitOfWork> _unitOfWork;
        
        public PaymentServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _bankService = new Mock<IBankService>();
            _paymentService = new PaymentService(_unitOfWork.Object, _bankService.Object);
        }
        
        [Fact]
        public async Task GetAllUserPaymentsAsync_AnyRequest_ReturnIEnumerableOfPaymentsAsync()
        {
            IEnumerable<Payment> payments = new List<Payment>();
            var userId = Guid.NewGuid();
            _unitOfWork.Setup(u => u.Payments.GetAllWithCurrencyByUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(payments);
                
            var result = await _paymentService.GetAllUserPaymentsAsync(userId);

            Assert.True(result is IEnumerable<Payment>);
        }
        
        [Fact]
        public async Task GetUserPaymentByIdAsync_ValidRequest_ReturnPayment()
        {
            var userId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            
            var payment = new Payment
            {
                Id = paymentId,
                UserId = userId
            };

            _unitOfWork.Setup(u => u.Payments.GetWithCurrencyByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
                
            var result = await _paymentService.GetUserPaymentByIdAsync(paymentId, userId);

            Assert.True(result is Payment);
        }
        
        [Fact]
        public async Task GetUserPaymentByIdAsync_NonExistingId_ThrowsHttpResponseExceptionWithStatusNotFound()
        {
            var userId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            
            Payment payment = null;

            _unitOfWork.Setup(u => u.Payments.GetWithCurrencyByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
                
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => _paymentService.GetUserPaymentByIdAsync(paymentId, userId));

            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }
        
        [Fact]
        public async Task GetUserPaymentByIdAsync_NotCorrectUserId_ThrowsHttpResponseExceptionWithStatusForbidden()
        {
            var userId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();
            
            Payment payment = new Payment
            {
                UserId = Guid.NewGuid(),
                Id = paymentId
            };

            _unitOfWork.Setup(u => u.Payments.GetWithCurrencyByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(payment);
                
            var result = await Assert.ThrowsAsync<HttpResponseException>(() => _paymentService.GetUserPaymentByIdAsync(paymentId, userId));

            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public async Task ProcessPaymentAsync_ValidRequestAndSuccessfulPayment_ReturnCreatedPaymentWithIdAndSuccesfulAsync()
        {
            var existingCurrency = new Currency { Id = Guid.NewGuid(), Name = "EUR" };
            _unitOfWork.Setup(u => u.Currencies.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCurrency);
            
            _unitOfWork.Setup(u => u.Payments.AddAsync(It.IsAny<Payment>()));

            var bankPaymentResult = new BankPaymentResult
            {
                Id = Guid.NewGuid(),
                Status = BankPaymentStatus.Succcessful
            };

            _bankService.Setup(b => b.ProcessPayment(It.IsAny<BankPayment>()))
                .ReturnsAsync(bankPaymentResult);

            var payment = new Payment
            {
                Amount = 100.00m,
                CardNumber = "0000000000000000",
                CurrencyId = existingCurrency.Id,
                Cvv = 111,
                ExpirationMonth = 10,
                ExpirationYear = 2030,
            };

            var userId = Guid.NewGuid();

            var result = await _paymentService.ProcessPaymentAsync(payment, userId);

            Assert.True(result.IsSuccessful);
            Assert.Equal(bankPaymentResult.Id, result.Id);
            Assert.Equal(userId, result.UserId);
            Assert.NotNull(result.CreatedAt);

            ResetMocks();
        }

        [Fact]
        public async Task ProcessPaymentAsync_UnsuccessfulPayment_ReturnPaymentWithIsSuccessfulFalseAsync()
        {
            var existingCurrency = new Currency { Id = Guid.NewGuid(), Name = "EUR" };
            _unitOfWork.Setup(u => u.Currencies.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(existingCurrency);
            
            _unitOfWork.Setup(u => u.Payments.AddAsync(It.IsAny<Payment>()));

            var bankPaymentResult = new BankPaymentResult
            {
                Id = Guid.NewGuid(),
                Status = BankPaymentStatus.Failed
            };

            _bankService.Setup(b => b.ProcessPayment(It.IsAny<BankPayment>()))
                .ReturnsAsync(bankPaymentResult);

            var payment = new Payment
            {
                Amount = 100.00m,
                CardNumber = "0000000000000000",
                CurrencyId = existingCurrency.Id,
                Cvv = 111,
                ExpirationMonth = 10,
                ExpirationYear = 2030,
            };

            var userId = Guid.NewGuid();

            var result = await _paymentService.ProcessPaymentAsync(payment, userId);

            Assert.False(result.IsSuccessful);

            ResetMocks();
        }

        [Fact]
        public async Task ProcessPaymentAsync_InvalidCurrency_ThrowsHttpResponseExceptionWithStatusBadRequest()
        {
            Currency nonExistingCurrency = null;
            _unitOfWork.Setup(u => u.Currencies.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(nonExistingCurrency);

            var payment = new Payment
            {
                CurrencyId = Guid.NewGuid(),
            };

            var userId = Guid.NewGuid();

            var result = await Assert.ThrowsAsync<HttpResponseException>(() => _paymentService.ProcessPaymentAsync(payment, userId));

            Assert.IsType(typeof(HttpResponseException), result);
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);

            ResetMocks();
        }

        private void ResetMocks()
        {
            _bankService.Reset();
            _unitOfWork.Reset();
        }
    }
}