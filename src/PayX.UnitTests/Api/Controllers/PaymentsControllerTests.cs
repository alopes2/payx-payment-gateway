using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PayX.Api.Controllers;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models;
using PayX.Core.Services;
using Xunit;

namespace PayX.UnitTests.Api.Controllers
{
    public class PaymentsControllerTests
    {
        private Mock<IPaymentService> _service;

        private Mock<IMapper> _mapper;

        private Mock<IValidator<ProcessPaymentResource>> _processValidator;

        private PaymentsController _controller;

        private Mock<ClaimsPrincipal> _context;

        private Guid _validUserId = Guid.NewGuid();

        public PaymentsControllerTests()
        {
            _service = new Mock<IPaymentService>();
            _mapper = new Mock<IMapper>();
            _processValidator = new Mock<IValidator<ProcessPaymentResource>>();
            _controller = new PaymentsController(_mapper.Object, _processValidator.Object, _service.Object);
            _context = new Mock<ClaimsPrincipal>();
        }

        [Fact]
        public async Task GetPaymentsAsync_ValidRequest_ReturnIEnumerableOfPaymentResourceResponse()
        {
            SetValidUserInControllerContext();
            _service
                .Setup(s => s.GetAllUserPaymentsAsync(_validUserId))
                .ReturnsAsync(It.IsAny<IEnumerable<Payment>>());
            
            var currencyResources = new List<PaymentResource>()
                .AsEnumerable();

            _mapper
                .Setup(m => m.Map<IEnumerable<Payment>, IEnumerable<PaymentResource>>(It.IsAny<IEnumerable<Payment>>()))
                .Returns(currencyResources);

            var response = await _controller.GetPaymentsAsync();

            var result = response.Result as ObjectResult;

            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.True(result.Value is IEnumerable<PaymentResource>);

            ResetMocks();
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ValidRequest_ReturnPaymentResourceResponse()
        {
            SetValidUserInControllerContext();
            
            var paymentResource = new PaymentResource();
            _mapper
                .Setup(m => m.Map<Payment, PaymentResource>(It.IsAny<Payment>()))
                .Returns(paymentResource);

            var response = await _controller.GetPaymentByIdAsync(Guid.NewGuid());

            var result = response.Result as ObjectResult;

            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.IsType(typeof(PaymentResource), result.Value);
        }

        [Fact]
        public async Task GetPaymentByIdAsync_NoPaymentId_ReturnBadRequest()
        {
            SetValidUserInControllerContext();

            var response = await _controller.GetPaymentByIdAsync(Guid.Empty);

            var result = response.Result as ObjectResult;

            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
        }

        private void ResetMocks()
        {
            _mapper.Reset();
            _service.Reset();
            _processValidator.Reset();
        }

        private void SetValidUserInControllerContext()
        {
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = _context.Object
                }
            };

            var claimsList = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, _validUserId.ToString())
            };

            _context.Setup(c => c.Claims)
                .Returns(claimsList);

            // Then set it to controller before executing test
            _controller.ControllerContext = context;
        }
    }
}