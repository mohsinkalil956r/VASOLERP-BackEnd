using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Assets;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetRepository _repository;
        public AssetsController(IAssetRepository repository)
        {
            this._repository = repository;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Object>> Get()
        {
            var assets = await this._repository.Get()
                .Include(p => p.Employees)
                .Include(p => p.AssetType)
                .ToListAsync();

            var result = assets.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.PurchaseDate,
                p.PurchasePrice,
                AssetType = new { p.AssetType.Id, p.AssetType.Name },
                Employees = p.Employees.Select(e => new { e.Id, e.FristName, e.LastName })

            }).ToList();

            return result;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var asset = await this._repository.Get(id)
                .Include(p => p.AssetType)
                .Include(p => p.Employees)
                .FirstOrDefaultAsync();

            if (asset != null)
            {
                var result = new
                {
                    asset.Name,
                    asset.Description,
                    asset.PurchaseDate,
                    asset.PurchasePrice,
                    AssetType = new { asset.AssetType.Id, asset.AssetType.Name },
                    Employees = asset.Employees.Select(e => new { e.Id, e.FristName, e.LastName })
                };
                return Ok(result);
            }
            return NotFound();
        }
         
        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] AssetPostVM model)
        {
            var asset = new Asset
            {
               Name = model.Name,
               Description = model.Description,
               PurchaseDate = model.PurchaseDate,
               PurchasePrice = model.PurchasePrice,
               AssetTypeId = model.AssetTypeId
            };

            _repository.Add(asset);
            await _repository.SaveChanges();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AssetPutVM model)
        {
            var asset = await this._repository.Get(id).Include(p => p.AssetIssuances).FirstOrDefaultAsync();

            if (asset != null)
            {
                asset.Name = model.Name;
                asset.AssetTypeId = model.AssetTypeId;
                asset.Description = model.Description;
                asset.PurchasePrice = model.PurchasePrice;
                asset.PurchaseDate = model.PurchaseDate;
                
                this._repository.Update(asset);
                await this._repository.SaveChanges();

                return Ok();
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var asset = await this._repository.Get(id).FirstOrDefaultAsync();
            if (asset != null)
            {
                asset.IsActive= false;
                await this._repository.SaveChanges();
            }
        }
    }
}
