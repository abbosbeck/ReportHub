using Domain.Entities;

namespace Application.Clients.RegisterClient;

public class RegisterClientProfile : Profile
{
    public RegisterClientProfile()
    {
        CreateMap<RegisterClientCommand, Client>();
        CreateMap<Client, ClientDto>();
    }
}
