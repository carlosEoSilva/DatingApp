using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    //-a rota deve começar com 'api/' seguida do nome do 'controller'.
    [Route("api/[controller]")]
    public class BaseApiController: ControllerBase {}
}