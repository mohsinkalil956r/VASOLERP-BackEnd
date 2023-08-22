using ERP.API.Models;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVersionController : ControllerBase
    {
        private readonly IVersionRepository _versionRepository;

        public ProductVersionController(IVersionRepository versionRepository)
        {
            _versionRepository = versionRepository;
        }

        [HttpGet]
        public IActionResult GetVersion()
        {
            string version = _versionRepository.GetVersion();

            var response = new ResponseAPIVM<string>
            {
                IsError = false,
                Message = "",
                data = version,

            };

            return Ok(response);
        }
    }
}
