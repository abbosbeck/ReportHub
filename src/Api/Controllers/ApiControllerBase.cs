using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public abstract class ApiControllerBase(ISender mediator) : ControllerBase
{
    protected ISender Mediator => mediator;
}