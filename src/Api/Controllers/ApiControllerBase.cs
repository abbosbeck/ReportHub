using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ISender Mediator;

    protected ApiControllerBase(ISender mediator)
    {
        Mediator = mediator;
    }
}