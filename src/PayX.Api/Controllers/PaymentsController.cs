using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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

        public PaymentsController(IMapper mapper, IPaymentService service)
        {
            _mapper = mapper;
            _service = service;
        }

        // GET: payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentResource>>> GetPayments()
        {
            var userIdClaim = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var userId = new Guid(userIdClaim.Value);
            
            var payments = await _service.GetAllUserPayments(userId);

            var paymentResources = 
                _mapper.Map<IEnumerable<Payment>, IEnumerable<PaymentResource>>(payments);

            return Ok(paymentResources);
        }

        // GET: payments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentResource>> GetPaymentById([FromRoute] Guid id)
        {
            var userIdClaim = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var userId = new Guid(userIdClaim.Value);

            var payment = await _service.GetUserPaymentById(id, userId);

            var paymentResource = _mapper.Map<Payment, PaymentResource>(payment);

            return Ok(paymentResource);
        }

        // POST: payments
        [HttpPost]
        public async Task<ActionResult<PaymentResource>> ProcessPayment([FromBody] ProcessPaymentResource processPaymentResource)
        {
            var userIdClaim = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier));
            var userId = new Guid(userIdClaim.Value);

            var paymentToProcess = _mapper.Map<ProcessPaymentResource, Payment>(processPaymentResource);
            paymentToProcess.UserId = userId;
            
            var newPayment = await _service.ProcessPayment(paymentToProcess);

            var payment = await _service.GetUserPaymentById(newPayment.Id, userId);

            var paymentResource = _mapper.Map<Payment, PaymentResource>(payment);

            return Ok(paymentResource);
        }
    }
}