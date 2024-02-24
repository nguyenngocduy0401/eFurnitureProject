using eFurnitureProject.Application.Commons;
using eFurnitureProject.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace eFurnitureProject.Application.Utils
{
    public static class GenerateJsonWebTokenString
    {
        public static string GenerateJsonWebToken(this User user, AppConfiguration appConfiguration, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim("Email" ,user.Email),
            };
            var token = new JwtSecurityToken(
                issuer: appConfiguration.JwtOptions.Issuer,
                audience: appConfiguration.JwtOptions.Audience,
                claims: claims,
                expires: now.AddMinutes(30),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
