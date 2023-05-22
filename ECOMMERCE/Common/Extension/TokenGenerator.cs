using DataAccessLayer.Common;
using DataAccessLayer.Models.Identity;
using ECOMMERCE.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace ECOMMERCE.Common.Extension
{
    public class TokenGenerator
    {
        public static string GenerateToken(TokenGenerateDetail user, IConfiguration configuration)
        {
            var claims = GetClaims(user);
            return GetToken(claims, configuration);
        }
        public static List<Claim> GetClaims(TokenGenerateDetail user)
        {
            List<Claim> claims = new List<Claim>
            {
                //new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(Constants.Id, user.UserId),
                new Claim(Constants.UniqueKey, user.UniqueKey),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.FullName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(Constants.FullName, user.FullName),
                new Claim(Constants.PhoneNumber, user.PhoneNumber),
                new Claim(Constants.UserName, user.UserName),
                new Claim(Constants.UserType, user.UserType.ToString()),
                new Claim(Constants.Address, user.Address),
            };
            return claims;
        }
        public static string GetToken(List<Claim> userClaims, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenDefination:JwtKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
            issuer: configuration["TokenDefination:JwtIssuer"],
            audience: configuration["TokenDefination:JwtAudience"],
            userClaims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(configuration["TokenDefination:JwtValidMinutes"])),
            signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}
