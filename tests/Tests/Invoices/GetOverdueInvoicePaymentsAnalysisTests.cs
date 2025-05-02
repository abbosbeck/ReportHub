using Application.Common.Exceptions;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Application.Invoices.GetOverdueInvoicePaymentsAnalysis;
using Domain.Entities;
using Domain.Enums;
using MockQueryable;
using Moq;

namespace Tests.Invoices;

[TestFixture]
public class GetOverdueInvoicePaymentsAnalysisTests
{
    private Mock<ICountryService> mockCountryServervice;
    private Mock<IDateTimeService> mockDateTimeService;
    private Mock<IClientRepository> mockClientRepository;
    private Mock<IInvoiceRepository> mockInvoiceRepository;
    private Mock<ICurrencyExchangeService> mockCurrencyExchangeService;
    private GetOverdueInvoicePaymentsAnalysisQueryHandler handler;

    [SetUp]
    public void Setup()
    {
        mockCountryServervice = new Mock<ICountryService>();
        mockDateTimeService = new Mock<IDateTimeService>();
        mockClientRepository = new Mock<IClientRepository>();
        mockInvoiceRepository = new Mock<IInvoiceRepository>();
        mockCurrencyExchangeService = new Mock<ICurrencyExchangeService>();

        handler = new GetOverdueInvoicePaymentsAnalysisQueryHandler(
            mockCountryServervice.Object,
            mockDateTimeService.Object,
            mockClientRepository.Object,
            mockInvoiceRepository.Object,
            mockCurrencyExchangeService.Object);
    }

    [Test]
    public async Task ShouldReturnCorrectOverdueInvoicePaymentsAnalysisAsync()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var today = DateTime.UtcNow.Date;
        var client = new Client
        {
            Id = clientId,
            CountryCode = "USA",
        };

        var invoices = new List<Invoice>
        {
            new ()
            {
                Amount = 1_000_000,
                CurrencyCode = "UZS",
                DueDate = today.AddDays(-1),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 1_200_000,
                CurrencyCode = "UZS",
                DueDate = today.AddDays(-2),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 200,
                CurrencyCode = "USD",
                DueDate = today.AddDays(-3),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 200,
                CurrencyCode = "USD",
                DueDate = today.AddDays(-3),
                PaymentStatus = InvoicePaymentStatus.Paid,
            },
            new ()
            {
                Amount = 120,
                CurrencyCode = "EUR",
                DueDate = today.AddDays(-3),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
        }
        .BuildMock()
        .AsQueryable();

        mockClientRepository
            .Setup(repository => repository.GetByIdAsync(clientId))
            .ReturnsAsync(client);

        mockCountryServervice
            .Setup(service => service.GetCurrencyCodeByCountryCodeAsync("USA"))
            .ReturnsAsync("USD");

        mockInvoiceRepository
            .Setup(repository => repository.GetAll())
            .Returns(invoices);

        mockDateTimeService
            .Setup(service => service.UtcNow)
            .Returns(today);

        mockCurrencyExchangeService
            .Setup(service => service.ExchangeCurrencyAsync("UZS", "USD", 2_200_000, today))
            .ReturnsAsync(175);

        mockCurrencyExchangeService
            .Setup(service => service.ExchangeCurrencyAsync("USD", "USD", 200, today))
            .ReturnsAsync(200);

        mockCurrencyExchangeService
            .Setup(service => service.ExchangeCurrencyAsync("EUR", "USD", 120, today))
            .ReturnsAsync(150);

        var query = new GetOverdueInvoicePaymentsAnalysisQuery(clientId);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result.CurrencyCode, Is.EqualTo("USD"));
            Assert.That(result.NumberOfInvoices, Is.EqualTo(4));
            Assert.That(result.TotalAmount, Is.EqualTo(525));
        });

        mockClientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        mockCountryServervice.Verify(
            service => service.GetCurrencyCodeByCountryCodeAsync(It.IsAny<string>()),
            Times.Once);

        mockInvoiceRepository.Verify(
            repository => repository.GetAll(),
            Times.Once);

        mockDateTimeService.Verify(
            service => service.UtcNow,
            Times.Once);

        mockCurrencyExchangeService.Verify(
            service => service.ExchangeCurrencyAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()),
            Times.Exactly(3));
    }

    [Test]
    public async Task ShouldThrowNotFoundExceptionWhenNoOverdueInvoicesFoundAsync()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var invoices = new List<Invoice>().BuildMock().AsQueryable();

        mockClientRepository
            .Setup(repository => repository.GetByIdAsync(clientId))
            .ReturnsAsync(new Client());

        mockCountryServervice
            .Setup(service => service.GetCurrencyCodeByCountryCodeAsync(string.Empty))
            .ReturnsAsync(string.Empty);

        mockDateTimeService
            .Setup(service => service.UtcNow)
            .Returns(DateTime.Today);

        mockInvoiceRepository
            .Setup(repository => repository.GetAll())
            .Returns(invoices);

        var query = new GetOverdueInvoicePaymentsAnalysisQuery(clientId);

        // Act and Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));

        mockClientRepository.Verify(
            repository => repository.GetByIdAsync(It.IsAny<Guid>()),
            Times.Once);

        mockCountryServervice.Verify(
            service => service.GetCurrencyCodeByCountryCodeAsync(It.IsAny<string>()),
            Times.Once);

        mockInvoiceRepository.Verify(
            repository => repository.GetAll(),
            Times.Once);

        mockDateTimeService.Verify(
            service => service.UtcNow,
            Times.Once);

        mockCurrencyExchangeService.Verify(
            service => service.ExchangeCurrencyAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateTime>()),
            Times.Never);

        await Task.CompletedTask;
    }
}