using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Constants;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Application.Common.Services;

public class JwtTokenGenerator(
        IOptions<JwtOptions> jwtOptions,
        IUserRepository userRepository,
        ISystemRoleAssignmentRepository systemRoleAssignmentRepository,
        IClientRoleAssignmentRepository clientRoleAssignmentRepository)
        : IJwtTokenGenerator
    {
        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var systemRoles = await systemRoleAssignmentRepository.GetRolesByUserIdAsync(user.Id);
            var clientRoles = await clientRoleAssignmentRepository.GetRolesByUserIdAsync(user.Id);
            var clinetRolesJson = JsonConvert.SerializeObject(clientRoles);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            claims.AddRange(systemRoles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(clientRoles.Select(role => new Claim("ClientRoles", clinetRolesJson)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtOptions.Value.AccessTokenExpiryMinutes));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays);
            await userRepository.UpdateAsync(user);
            return refreshToken;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }