using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CustomIdentity.BLL.Constants;
using CustomIdentity.BLL.Services.Interfaces;
using CustomIdentity.DAL.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CustomIdentity.BLL.Services.Implementation
{
    public class TokenGeneratorService : ITokenGeneratorService
    {
        private readonly SymmetricSecurityKey _securityKey;
        private readonly IConfiguration _configuration;

        public TokenGeneratorService(IConfiguration configuration)
        {
            var tokenKey = configuration[TokenConstants.JwtSecret];
            _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            _configuration = configuration;
        }

        public string CreateJwtToken(User userModel)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, userModel.UserName)
            };

            var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);
            var tokenDuration = double.Parse(_configuration[TokenConstants.JwtTokenDurationHours]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(tokenDuration),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
