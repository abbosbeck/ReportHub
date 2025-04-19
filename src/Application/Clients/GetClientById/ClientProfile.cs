using Domain.Entities;

namespace Application.Clients.GetClientById;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();
    }
}
