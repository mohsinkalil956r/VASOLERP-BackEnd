using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetVersion()
        {
            string version = "1.0.0"; // Hardcoded version

            return Ok(new { Version = version });
        }
    }
}
