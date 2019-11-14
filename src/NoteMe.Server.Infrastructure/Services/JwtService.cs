using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Extensions;
using NoteMe.Server.Infrastructure.Services.Common;
using NoteMe.Server.Infrastructure.Settings;

namespace NoteMe.Server.Infrastructure.Services
{
    public interface IJwtService : IService
    {
        JwtDto CreateToken(UserDto user);
    }
    
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        
        private static ILogger _logger = LogManager.GetCurrentClassLogger();

        public JwtService(JwtSettings settings)
        {
            _settings = settings;
        }
        
        public JwtDto CreateToken(UserDto user)
        {
            _logger.Debug($"Creating access token for user: #{user.Id}");

            var now = DateTime.UtcNow;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString(), ClaimValueTypes.Integer64),
            };
            

            var expires = now.AddMinutes(_settings.ExpiryMinutes);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key)),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _settings.Issuer,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: signingCredentials
            );

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwt);

            var jwtDto = new JwtDto
            {
                User = user,
                Token = token,
                Expires = expires.ToUniversalTime()
            };

            return jwtDto;
        }
    }
}