using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PayX.Api.Configurations;
using PayX.Api.Controllers.Resources;
using PayX.Core.Models.Auth;
using PayX.Core.Services;

namespace PayX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;

        private readonly IAuthService _service;
        private readonly IValidator<AuthResource> _validator;

        public AuthController(
            IAuthService service,
            IOptionsSnapshot<JwtSettings> jwtSettings,
            IValidator<AuthResource> validator)
        {
            _service = service;
            _validator = validator;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost("signup")]
        public async Task<ActionResult<string>> SignUp([FromBody] AuthResource credentials)
        {
            var validationResult = _validator.Validate(credentials);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var user = await _service.SignUpAsync(credentials.Email, credentials.Password);

            return GenerateJwtToken(user);
        }

        [HttpPost("signin")]
        public async Task<ActionResult<string>> SignIn([FromBody] AuthResource credentials)
        {
            var validationResult = _validator.Validate(credentials);
            if(!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            
            var user = await _service.SignInAsync(credentials.Email, credentials.Password);

            return GenerateJwtToken(user);
        }


        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtSettings.ExpirationInDays));

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Issuer,
                claims,
                expires : expires,
                signingCredentials : creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}