using Api.Controllers;
using Application.WeatherForecast.GetWeatherForecast;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests;

public class WeatherControllerTest
{
    private WeatherForecastController _controller;
    private Mock<ISender> _mediatorMock;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = new Mock<ISender>();
        var loggerMock = new Mock<ILogger<WeatherForecastController>>();

        _controller = new WeatherForecastController(loggerMock.Object);
    }

    [Test]
    public async Task GetWeatherForecastAsync_ReturnsOkWithData()
    {
        // Arrange
        var fakeData = new List<WeatherForecast>
        {
            new() { TemperatureC = 20, Summary = "Sunny" },
            new() { TemperatureC = 15, Summary = "Cloudy" }
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<GetWeatherForecastRequest>(), default))
            .ReturnsAsync(fakeData);

        // Act
        await _controller.GetWeatherForecastAsync();

        // Assert
        Assert.Pass();
    }
}
