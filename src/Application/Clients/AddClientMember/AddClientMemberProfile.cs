using Domain.Entities;

namespace Application.Clients.AddClientMember;

public class AddClientMemberProfile : Profile
{
    public AddClientMemberProfile()
    {
        CreateMap<AddClientMemberCommand, ClientDto>();
        CreateMap<ClientDto, Client>();
    }
}
