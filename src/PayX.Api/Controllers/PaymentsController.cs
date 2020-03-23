using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PayX.Api.Controllers.Resources;
using PayX.Api.Models;
using PayX.Core.Extensions;
using PayX.Core.Models;
using PayX.Core.Services;

namespace PayX.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = Policies.CanManagePayments)]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _service;

        private readonly IMapper _mapper;

        private readonly IValidator<ProcessPaymentResource> _processValidator;

        public PaymentsController(IMapper mapper, IValidator<ProcessPaymentResource> processValidator, IPaymentService service)
        {
            _processValidator = processValidator;
            _mapper = mapper;
            _service = service;
        }

        // GET: payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResource>>> GetPaymentsAsync()
        {
            var userIdClaim = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var userId = new Guid(userIdClaim.Value);

            var payments = await _service.GetAllUserPaymentsAsync(userId);

            var paymentResources =
                _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentResource>>(payments);

            return Ok(paymentResources);
        }

        // GET: payments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResource>> GetPaymentByIdAsync([FromRoute] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id not provided.");
            }

            var userIdClaim = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var userId = new Guid(userIdClaim.Value);

            var payment = await _service.GetUserPaymentByIdAsync(id, userId);

            var paymentResource = _mapper.Map<Payment, PaymentResource>(payment);

            return Ok(paymentResource);
        }

        // POST: payments
        [HttpPost]
        public async Task<ActionResult<PaymentResource>> ProcessPaymentAsync([FromBody] ProcessPaymentResource processPaymentResource)
        {
            var validationResult = _processValidator.Validate(processPaymentResource);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var userIdClaim = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var userId = new Guid(userIdClaim.Value);

            var paymentToProcess = _mapper.Map<ProcessPaymentResource, Payment>(processPaymentResource);

            var newPayment = await _service.ProcessPaymentAsync(paymentToProcess, userId);

            var payment = await _service.GetUserPaymentByIdAsync(newPayment.Id, userId);

            var paymentResource = _mapper.Map<Payment, PaymentResource>(payment);

            return Ok(paymentResource);
        }
    }
}