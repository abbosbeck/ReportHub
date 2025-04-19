using Domain.Entities;

namespace Application.Clients.UpdateClient;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();
    }
}
