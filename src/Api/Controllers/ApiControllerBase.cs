using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase(ISender mediator) : ControllerBase
{
    protected ISender Mediator => mediator;
}