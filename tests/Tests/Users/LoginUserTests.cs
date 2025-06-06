﻿using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Application.Users.LoginUser;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Tests.Users;

public class LoginUserTests
{
    private Mock<IUserRepository> userRepository;
    private Mock<IPasswordHasher<User>> passwordHasher;
    private Mock<IJwtTokenGenerator> mockJwtTokenGenerator;
    private Mock<IValidator<LoginUserCommand>> mockValidator;
    private Mock<IDateTimeService> mockDateTimeService;
    private LoginUserCommandHandler handler;

    [SetUp]
    public void Setup()
    {
        userRepository = new Mock<IUserRepository>();
        passwordHasher = new Mock<IPasswordHasher<User>>();
        mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
        mockValidator = new Mock<IValidator<LoginUserCommand>>();
        mockDateTimeService = new Mock<IDateTimeService>();

        handler = new LoginUserCommandHandler(
            userRepository.Object,
            passwordHasher.Object,
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

        userRepository
            .Setup(r => r.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, command.Password))
            .Returns(PasswordVerificationResult.Success);

        mockJwtTokenGenerator
            .Setup(j => j.GenerateAccessTokenAsync(user))
            .ReturnsAsync("access_token");

        mockJwtTokenGenerator
            .Setup(j => j.GenerateRefreshToken())
            .Returns("refresh_token");

        userRepository
            .Setup(r => r.UpdateAsync(user))
            .ReturnsAsync(user);

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

        userRepository
            .Setup(r => r.GetByEmailAsync(user.Email))
            .ReturnsAsync(user);

        passwordHasher
            .Setup(h => h.VerifyHashedPassword(user, user.PasswordHash, command.Password))
            .Returns(PasswordVerificationResult.Failed);

        Assert.ThrowsAsync<UnauthorizedException>(async () =>
        {
            await handler.Handle(command, CancellationToken.None);
        });
    }
}