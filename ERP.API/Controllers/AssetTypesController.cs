using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.AssetType;
using System.Runtime.CompilerServices;
using ERP.API.Models;
using System.Xml.Linq;

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

        // GET api/<ValuesController>/5
        [HttpGet]
        public async Task<APIResponse<Object>> Get()
        {
            var assets = await this._repository.Get().ToListAsync();

            var result = assets.Select(p => new
            {
                p.Id,
                p.Name,
                Assets = p.Assets.Select(a => new
                {
                    a.Name,
                    a.PurchaseDate,
                    a.PurchasePrice,
                    a.Description,
                })

            }).ToList();

            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = result
            };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<APIResponse<object>> Get(int id)
        {
            var assetType = await this._repository.Get(id).Include(p => p.Assets).FirstOrDefaultAsync();
            if (assetType != null)
            {
                var vm = new AssetTypeGetVM
                {
                    Name = assetType.Name,
                };
                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = vm
                };
            }
            return new APIResponse<object>
            {
                IsError = false,
                Message = "",

            };
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
        public async Task<APIResponse<object>> Put(int id, [FromBody] AssetTypePostVM model)
        {
            var assetType = await this._repository.Get(id).FirstOrDefaultAsync();

            if (assetType != null)
            {
                assetType.Name = model.Name;

                this._repository.Update(assetType);
                await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                };
            }

            return new APIResponse<object>
            {
                IsError = false,
                Message = "",

            };

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            var assetType = await this._repository.Get(id).FirstOrDefaultAsync();
            if (assetType != null)
            {
                assetType.IsActive = false;
                await this._repository.SaveChanges();
                return Ok();

            }
            return NotFound();
        }
    }  }