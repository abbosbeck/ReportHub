using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Time;
using Application.Users.LoginUser;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests;

public class LoginUserTests
{
    private Mock<UserManager<User>> mockUserManager;
    private Mock<IJwtTokenGenerator> mockJwtTokenGenerator;
    private Mock<IValidator<LoginUserCommand>> mockValidator;
    private Mock<IDateTimeService> mockDateTimeService;
    private LoginUserCommandHandler handler;

    [SetUp]
    public void Setup()
    {
        mockUserManager = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null);
        mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        mockValidator = new Mock<IValidator<LoginUserCommand>>();
        mockDateTimeService = new Mock<IDateTimeService>();

        handler = new LoginUserCommandHandler(
            mockUserManager.Object,
            mockDateTimeService.Object,
            mockJwtTokenGenerator.Object,
            mockValidator.Object);
    }

    [Test]
    public async Task ShouldAuthenticateSuccessfully_WhenValidCredentials()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@gmail.com",
            PasswordHash = "hashed_password",
            EmailConfirmed = true,
        };

        var command = new LoginUserCommand
        {
            Email = user.Email,
            Password = "password123",
        };

        mockUserManager
            .Setup(r => r.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);

        mockUserManager
            .Setup(h => h.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(true);

        mockJwtTokenGenerator
            .Setup(j => j.GenerateAccessTokenAsync(user))
            .ReturnsAsync("access_token");

        mockJwtTokenGenerator
            .Setup(j => j.GenerateRefreshToken())
            .Returns("refresh_token");

        mockUserManager
            .Setup(r => r.UpdateAsync(user))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.AccessToken, Is.EqualTo("access_token"));
        Assert.That(result.RefreshToken, Is.EqualTo("refresh_token"));
    }

    [Test]
    public void ShouldThrowException_WhenInvalidPassword()
    {
        var user = new User
        {
            Email = "user@gmail.com",
            PasswordHash = "hashed_pass",
            EmailConfirmed = true,
        };

        var command = new LoginUserCommand
        {
            Email = user.Email,
            Password = "wrong_password",
        };

        mockUserManager
            .Setup(r => r.FindByEmailAsync(user.Email))
            .ReturnsAsync(user);

        mockUserManager
            .Setup(h => h.CheckPasswordAsync(user, command.Password))
            .ReturnsAsync(false);

        Assert.ThrowsAsync<UnauthorizedException>(async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        });
    }
}