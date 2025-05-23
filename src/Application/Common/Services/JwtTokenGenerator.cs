﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Configurations;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application.Common.Services;

public class JwtTokenGenerator(
        IUserRepository userRepository,
        IOptions<JwtOptions> jwtOptions,
        IDateTimeService dateTimeService,
        ISystemRoleAssignmentRepository systemRoleAssignmentRepository,
        IClientRoleAssignmentRepository clientRoleAssignmentRepository)
        : IJwtTokenGenerator
    {
        public async Task<string> GenerateAccessTokenAsync(User user)
        {
            var systemRoles = await systemRoleAssignmentRepository.GetRolesByUserIdAsync(user.Id);
            var clientRoles = await clientRoleAssignmentRepository.GetByUserIdAsync(user.Id);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new (JwtRegisteredClaimNames.Email, user.Email!),
            };

            claims.AddRange(systemRoles.Select(role => new Claim("SystemRoles", role)));
            claims.AddRange(clientRoles.Select(role => new Claim("ClientRoles", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = dateTimeService.UtcNow.AddMinutes(Convert.ToDouble(jwtOptions.Value.AccessTokenExpiryMinutes));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user)
        {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = dateTimeService.UtcNow.AddDays(jwtOptions.Value.RefreshTokenExpiryDays);
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