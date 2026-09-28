using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Dto;
using Domain.Interfaces.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace InfraStructure.Authentication
{
   public class JwtService : IJwtTokenService
    {
        private readonly byte[] key;
       public JwtService()
        {
            key = Encoding.ASCII.GetBytes(JwtSettings.key);
        }
        public TokenDto GenerateJwtToken(int userid,string username,string email, int roleId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
          

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("username", username),
                    new Claim("email", email),
                    new Claim("userid", userid.ToString()),
                    new Claim("roleid", roleId.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(JwtSettings.hours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            TokenDto tokenDto = new TokenDto()
            {
                JWTToken = tokenHandler.WriteToken(token),
                RefreshToken = GenerateRefreshToken(username),
                ExpireTime = token.ValidTo

            };
            return tokenDto;
        }

        public string ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "username")?.Value;
  
                return username;
            }
            catch (Exception ex)
            {
                // if validation fails then return null
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public string GenerateRefreshToken(string username)
        {
            byte[] randomNumber=new byte[64];
            var randomnumbergenerator = RandomNumberGenerator.Create();
            randomnumbergenerator.GetBytes(randomNumber);
            string refreshToken = Convert.ToBase64String(randomNumber);
            return refreshToken;
        }

    }
}
