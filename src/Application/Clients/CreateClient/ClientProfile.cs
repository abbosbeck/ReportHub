using Domain.Entities;

namespace Application.Clients.CreateClient;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();
    }
}
