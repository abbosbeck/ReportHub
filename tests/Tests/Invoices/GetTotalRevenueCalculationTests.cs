using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Application.Invoices.GetTotalRevenueCalculation;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence.Repositories;
using MockQueryable;
using Moq;

namespace Tests.Invoices;

[TestFixture]
public class GetTotalRevenueCalculationTests
{
    private Mock<IClientRepository> mockClientRepository;
    private Mock<IInvoiceRepository> mockInvoiceRepository;
    private Mock<ICountryService> mockCountryService;
    private Mock<ICurrencyExchangeService> mockCurrencyExchangeService;
    private GetTotalRevenueCalculationQueryHandler handler;

    [SetUp]
    public void Setup()
    {
        mockClientRepository = new Mock<IClientRepository>();
        mockInvoiceRepository = new Mock<IInvoiceRepository>();
        mockCountryService = new Mock<ICountryService>();
        mockCurrencyExchangeService = new Mock<ICurrencyExchangeService>();

        handler = new GetTotalRevenueCalculationQueryHandler(
            mockClientRepository.Object,
            mockInvoiceRepository.Object,
            mockCountryService.Object,
            mockCurrencyExchangeService.Object);
    }

    [Test]
    public async Task Should_Return_ValidRevenue()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-90);
        var endDate = today.AddDays(90);

        var client = new Client
        {
            Id = clientId,
            CountryCode = "UZB",
        };

        var invoices = new List<Invoice>()
        {
            new ()
            {
                Amount = 1_000_000,
                CurrencyCode = "UZS",
                IssueDate = today.AddDays(-20),
                DueDate = today.AddDays(20),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 1_200_000,
                CurrencyCode = "UZS",
                IssueDate = today.AddDays(-18),
                DueDate = today.AddDays(4),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 200,
                CurrencyCode = "USD",
                IssueDate = today.AddDays(-30),
                DueDate = today.AddDays(30),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
        }
        .BuildMock()
        .AsQueryable();

        mockClientRepository
            .Setup(repository => repository.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        mockCountryService
            .Setup(service => service.GetCurrencyCodeByCountryCodeAsync("UZB"))
            .ReturnsAsync("UZS");

        mockInvoiceRepository
            .Setup(repository => repository.GetAll())
            .Returns(invoices);

        mockCurrencyExchangeService
            .Setup(service => service.ExchangeCurrencyAsync("UZS", "UZS", 1_000_000, today.AddDays(-20)))
            .ReturnsAsync(1_000_000);

        mockCurrencyExchangeService
            .Setup(service => service.ExchangeCurrencyAsync("UZS", "UZS", 1_200_000, today.AddDays(-18)))
            .ReturnsAsync(1_200_000);

        mockCurrencyExchangeService
            .Setup(service => service.ExchangeCurrencyAsync("USD", "UZS", 200, today.AddDays(-30)))
            .ReturnsAsync(2_000_000);

        mockCurrencyExchangeService
            .Setup(service => service.GetAmountWithSymbol(4_200_000, "UZS"))
            .Returns("4 200 000 so'm");

        var query = new GetTotalRevenueCalculationQuery(clientId, startDate, endDate);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.CurrencyCode, Is.EqualTo("UZS"));
            Assert.That(result.TotalRevenue, Is.EqualTo("4 200 000 so'm"));
        });

        mockClientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        mockCountryService.Verify(
            service => service.GetCurrencyCodeByCountryCodeAsync(It.IsAny<string>()),
            Times.Once);

        mockInvoiceRepository.Verify(
            repository => repository.GetAll(),
            Times.Once);

        mockCurrencyExchangeService.Verify(
            service => service.ExchangeCurrencyAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()),
            Times.Exactly(3));
    }
}
