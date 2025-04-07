using Application.Common.Interfaces;
using Application.Users.LoginUser;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;

namespace Tests;

public class LoginUserTests
{
    private Mock<IUserRepository> _mockUserRepository;
    private Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
    private Mock<IPasswordHasher<User>> _mockPasswordHasher;
    private LoginUserQueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        _mockPasswordHasher = new Mock<IPasswordHasher<User>>();

        _handler = new LoginUserQueryHandler(
            _mockUserRepository.Object,
            _mockJwtTokenGenerator.Object,
            _mockPasswordHasher.Object
        );
    }

    [Test]
    public async Task ShouldAuthenticateSuccessfully_WhenValidCredentials()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            PhoneNumber = "+998909009090",
            PasswordHash = "hashed_password"
        };

        var query = new LoginUserQuery
        {
            PhoneNumber = user.PhoneNumber,
            Password = "password123"
        };

        _mockUserRepository
            .Setup(r => r.GetUserByPhoneNumberAsync(user.PhoneNumber))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, query.Password))
            .Returns(PasswordVerificationResult.Success);

        _mockJwtTokenGenerator
            .Setup(j => j.GenerateAccessTokenAsync(user))
            .ReturnsAsync("access_token");

        _mockJwtTokenGenerator
            .Setup(j => j.GenerateRefreshToken())
            .Returns("refresh_token");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result.AccessToken, Is.EqualTo("access_token"));
        Assert.That(result.RefreshToken, Is.EqualTo("refresh_token"));
    }

    [Test]
    public void ShouldThrowException_WhenInvalidPassword()
    {
        var user = new User
        {
            PhoneNumber = "+9989009090",
            PasswordHash = "hashed_pass"
        };

        var command = new LoginUserQuery
        {
            PhoneNumber = user.PhoneNumber,
            Password = "wrong_password"
        };

        _mockUserRepository
            .Setup(r => r.GetUserByPhoneNumberAsync(user.PhoneNumber))
            .ReturnsAsync(user);

        _mockPasswordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, command.Password))
            .Returns(PasswordVerificationResult.Failed);

        Assert.ThrowsAsync<UnauthorizedException>(async () =>
        {
            await _handler.Handle(command, CancellationToken.None);
        });
    }
}
