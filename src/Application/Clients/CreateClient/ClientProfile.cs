using Domain.Entities;

namespace Application.Clients.CreateClient;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientCommand, Client>();
    }
}
