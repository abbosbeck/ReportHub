using Application.Common.Exceptions;
using Application.Common.Interfaces.Repositories;
using Application.Invoices.GetTotalNumberOfInvoices;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MockQueryable;
using Moq;

namespace Tests.Invoices;

[TestFixture]
public class GetTotalNumberOfInvoicesTests
{
    private Mock<IInvoiceRepository> mockInvoiceRepository;
    private Mock<IValidator<GetInvoiceCountQuery>> mockValidator;
    private GetInvoiceCountQueryHandler handler;

    [SetUp]
    public void Setup()
    {
        mockInvoiceRepository = new Mock<IInvoiceRepository>();
        mockValidator = new Mock<IValidator<GetInvoiceCountQuery>>();

        handler = new GetInvoiceCountQueryHandler(
            mockInvoiceRepository.Object,
            mockValidator.Object);
    }

    [Test]
    public async Task Should_Return_ValidNumberOfInvoices()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-90);
        var endDate = today.AddDays(90);

        var invoices = new List<Invoice>
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

        mockInvoiceRepository
            .Setup(repository => repository.GetAll())
            .Returns(invoices);

        var query = new GetInvoiceCountQuery(clientId, startDate, endDate, null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(3));

        mockInvoiceRepository.Verify(
            repository => repository.GetAll(),
            Times.Once);
    }

    [Test]
    public async Task Should_Throw_NotFoundException_WhenNoInvoicesFound()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-90);
        var endDate = today.AddDays(0);

        var invoices = new List<Invoice>
        {
            new ()
            {
                Amount = 1_000_000,
                CurrencyCode = "UZS",
                IssueDate = today.AddDays(20),
                DueDate = today.AddDays(20),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 1_200_000,
                CurrencyCode = "UZS",
                IssueDate = today.AddDays(18),
                DueDate = today.AddDays(4),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
            new ()
            {
                Amount = 200,
                CurrencyCode = "USD",
                IssueDate = today.AddDays(30),
                DueDate = today.AddDays(30),
                PaymentStatus = InvoicePaymentStatus.Unpaid,
            },
        }
        .BuildMock()
        .AsQueryable();

        mockInvoiceRepository
            .Setup(repository => repository.GetAll())
            .Returns(invoices);

        var query = new GetInvoiceCountQuery(clientId, startDate, endDate, null);

        // Act and Assert
        Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(query, CancellationToken.None));

        mockInvoiceRepository.Verify(
            repository => repository.GetAll(),
            Times.Once);

        await Task.CompletedTask;
    }
}