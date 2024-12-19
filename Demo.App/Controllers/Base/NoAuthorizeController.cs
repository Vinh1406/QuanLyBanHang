using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.App.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoAuthorizeController : ControllerBase
    {
    }
}
