using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.Exceptions;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;

namespace Application.Plans.GetPlanById;

public class GetPlanByIdQuery(Guid planId, Guid clientId) : IRequest<PlanDto>, IClientRequest
{
    public Guid Id { get; set; } = planId;

    public Guid ClientId { get; set; } = clientId;
}

[RequiresClientRole(ClientRoles.Owner, ClientRoles.ClientAdmin)]
public class GetPlanByIdQueryHandler(
    IPlanRepository planRepository,
    IPlanItemRepository planItemRepository,
    IClientRepository clientRepository,
    ICountryService countryService,
    IItemRepository itemRepository,
    ICurrencyExchangeService currencyExchangeService,
    IMapper mapper)
    : IRequestHandler<GetPlanByIdQuery, PlanDto>
{
    public async Task<PlanDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var plan = await planRepository.GetByIdAsync(request.Id)
            ?? throw new NotFoundException($"Plan is not found with this id: {request.ClientId}");

        var client = await clientRepository.GetByIdAsync(plan.ClientId)
            ?? throw new NotFoundException($"Client is not found with this id: {plan.ClientId}");

        var clientCurrency = await countryService.GetCurrencyCodeByCountryCodeAsync(client.CountryCode);

        var planItems = await planItemRepository.GetPlanItemsByPlanIdAsync(plan.Id);

        foreach (var planItem in planItems)
        {
            var item = await itemRepository.GetByIdAsync(planItem.ItemId);

            var price = await currencyExchangeService
                .ExchangeCurrencyAsync(item.CurrencyCode, clientCurrency, item.Price, plan.StartDate);

            plan.TotalPrice += planItem.Quantity * price;
            plan.Items.Add(item);
        }

        return mapper.Map<PlanDto>(plan);
    }
}