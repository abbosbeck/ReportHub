using Api.Controllers;
using Application.WeatherForecast.GetWeatherForecast;
using MediatR;
using Moq;

namespace Tests;

public class WeatherControllerTest
{
    private WeatherForecastController controller;
    private Mock<ISender> mediatorMock;

    [SetUp]
    public void SetUp()
    {
        mediatorMock = new Mock<ISender>();

        controller = new WeatherForecastController(mediatorMock.Object);
    }

    [Test]
    public async Task GetWeatherForecastAsync_ReturnsOkWithData()
    {
        // Arrange
        var fakeData = new List<WeatherForecast>
        {
            new () { TemperatureC = 20, Summary = "Sunny" },
            new () { TemperatureC = 15, Summary = "Cloudy" },
        };

        mediatorMock
            .Setup(m => m.Send(It.IsAny<GetWeatherForecastRequest>(), default))
            .ReturnsAsync(fakeData);

        // Act
        await controller.GetWeatherForecastAsync();

        // Assert
        Assert.Fail();
    }
}
