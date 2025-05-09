using Domain.Entities;

namespace Application.Clients.GetClientByUserId;

public class ClientProfile : Profile
{
    public ClientProfile()
    {
        CreateMap<Client, ClientDto>();
    }
}
