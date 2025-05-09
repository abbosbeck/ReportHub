using Domain.Entities;

namespace Application.Clients.GetClientsList;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();
    }
}
