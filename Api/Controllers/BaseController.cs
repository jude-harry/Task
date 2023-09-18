namespace Api.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class BaseController : ControllerBase
{
    public BaseController()
    {

    }

}

