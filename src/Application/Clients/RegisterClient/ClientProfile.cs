using Domain.Entities;

namespace Application.Clients.RegisterClient;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<RegisterClientCommand, Client>();
        CreateMap<Client, ClientDto>();
    }
}
