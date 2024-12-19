using Demo.Domain.InfrastructureServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Infrastructure
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IOptions<JwtOption> _options;
        private readonly IConfiguration _configuration;
        public JwtTokenService(IOptions<JwtOption> options, IConfiguration configuration)
        {
            _options = options;
            _configuration = configuration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            try
            {
                var a = _options.Value.SecretKey;
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                    issuer: _options.Value.Issuer,
                    audience: _options.Value.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(_options.Value.ExpireMin),
                    signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return tokenString;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
