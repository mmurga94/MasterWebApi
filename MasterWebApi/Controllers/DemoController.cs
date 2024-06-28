namespace MasterWebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("Demo")]
    public class DemoController : ControllerBase
    {
        [HttpGet("GetString")]
        public string GetNombre()
        {
            return "tendenco.com";
        }
    }
}
