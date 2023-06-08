using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.AssetType;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypesController : ControllerBase
    {
        private readonly IAssetTypeRepository _repository;
        public AssetTypesController(IAssetTypeRepository repository)
        {
            this._repository = repository;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Object>> Get()
        {
            var assetType = await this._repository.Get().Include(p=>p.Assets).ToListAsync();


            var result = assetType.Select(p => new
            {
                   p.Id,
                   p.Name,
                 Assets=  p.Assets.Select(a => new { a.Name, a.PurchaseDate,a.PurchasePrice,a.Description,
                 })

            }).ToList();

            return result;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var assetType = await this._repository.Get(id).Include(p => p.Assets).FirstOrDefaultAsync();
            if (assetType!=null) {
                var vm = new AssetTypeGetVM
                {
                    Name = assetType.Name,
                };
                return Ok(vm);
            }
            return BadRequest();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] AssetTypePostVM model)
        {
            var assetType = new AssetType
            {
                Name = model.Name,
            
            };

            _repository.Add(assetType);
            await _repository.SaveChanges();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AssetTypePostVM model)
        {
            var assetType = await this._repository.Get(id).Include(p => p.Assets).FirstOrDefaultAsync();

            if (assetType!=null)
            {
            assetType.Name=model.Name;

                this._repository.Update(assetType);
                await this._repository.SaveChanges();

                return Ok();
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult>  Delete(int id)
        {

            var assetType = await this._repository.Get(id).FirstOrDefaultAsync();
            if (assetType!=null)
            {
                assetType.IsActive=false;
                await this._repository.SaveChanges();

                return Ok();

            }
            return NotFound();
        }
    }
}
