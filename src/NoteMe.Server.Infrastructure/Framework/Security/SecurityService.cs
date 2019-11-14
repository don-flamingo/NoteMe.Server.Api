using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Exceptions;
using NoteMe.Common.Extensions;

namespace NoteMe.Server.Infrastructure.Framework.Security
{
    public interface ISecurityService
    {
        string GetSalt();
        string GetHash(string value, string salt);
        JwtDto GetJwt(UserDto user, Guid id);
    }
    
    public class SecurityService : ISecurityService
    {
        private const int BytesIterationsCount = 1000;
        private const int SaltSize = 40;

        private readonly SecuritySettings _securitySettings;

        public SecurityService(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }

        public string GetHash(string value, string salt)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(salt))
            {
                throw new ServerException(ErrorCodes.InvalidParameter, $"salt: {salt}, value {value} ");
            }

            var pbkdf2 = new Rfc2898DeriveBytes(value, GetBytes(salt), BytesIterationsCount);
            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }

        public JwtDto GetJwt(UserDto user, Guid id)
        {
            var now = DateTime.UtcNow;
            var key = Encoding.ASCII.GetBytes(_securitySettings.Key);
            var expiresAt = now + _securitySettings.TokenDuration;
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString(), ClaimValueTypes.Integer64),
                }),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new JwtDto
            {
                Id = id,
                User = user,
                Token = tokenHandler.WriteToken(token),
                Expires = expiresAt
            };
        }

        public string GetSalt()
        {
            var saltBytes = new byte[SaltSize];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }
    }
}