using Domain.Entities;

namespace Application.Clients.CreateClient;

public class CreateClientProfile : Profile
{
    public CreateClientProfile()
    {
        CreateMap<Client, ClientDto>();
    }
}
