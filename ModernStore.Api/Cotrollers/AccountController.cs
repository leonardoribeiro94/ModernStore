using FluentValidator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ModernStore.Api.Security;
using ModernStore.Domain.Commands.Inputs;
using ModernStore.Domain.Entities;
using ModernStore.Domain.Repositories;
using ModernStore.Infra.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ModernStore.Api.Cotrollers
{
    public class AccountController : BaseController
    {
        private Customer _customer;
        private readonly ICustomerRepository _customerRepository;
        private readonly TokenOptions _tokenOptions;
        private readonly JsonSerializerSettings _serializerSettings;


        public AccountController(IOptions<TokenOptions> jwtOptions, IUow uow,
            ICustomerRepository repository) : base(uow)
        {
            _customerRepository = repository;
            _tokenOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_tokenOptions);

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("v1/autenticate")]
        public async Task<IActionResult> Post([FromForm] AuthenticateUserCommand command)
        {
            if (command == null)
                return await Response(null,
                    new List<Notification> { new Notification("User", "Usuário ou senha invlálidas") });

            var identity = await GetClaims(command);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, command.UserName),
                new Claim(JwtRegisteredClaimNames.NameId, command.UserName),
                new Claim(JwtRegisteredClaimNames.Email, command.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, command.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, await _tokenOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_tokenOptions.IssuedAt).ToString(),
                    ClaimValueTypes.Integer64),
                identity.FindFirst("ModernStore")
            };

            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: claims.AsEnumerable(),
                notBefore: _tokenOptions.NotBefore,
                expires: _tokenOptions.Expiration,
                signingCredentials: _tokenOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                token = encodedJwt,
                expires = (int)_tokenOptions.ValidFor.TotalSeconds,
                user = new
                {
                    id = _customer.Id,
                    name = _customer.Name.ToString(),
                    email = _customer.Email.Address,
                    username = _customer.User.UserName
                }
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private static void ThrowIfInvalidOptions(TokenOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
                throw new ArgumentException("O período deve ser maior que zero", nameof(TokenOptions.ValidFor));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenOptions.SigningCredentials));

            if (options.JtiGenerator == null)
                throw new ArgumentNullException(nameof(TokenOptions.JtiGenerator));
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


        private Task<ClaimsIdentity> GetClaims(AuthenticateUserCommand command)
        {

            var customer = _customerRepository.GetByUserName(command.UserName);

            if (customer == null)
                return Task.FromResult<ClaimsIdentity>(null);

            if (!customer.User.Authenticate(command.UserName, command.Password))
                return Task.FromResult<ClaimsIdentity>(null);

            _customer = customer;


            return Task.FromResult(new ClaimsIdentity(
                new GenericIdentity(customer.User.UserName, "Token"),
                new[]
                {
                    new Claim("ModernStore", "User")
                }));
        }

    }
}
