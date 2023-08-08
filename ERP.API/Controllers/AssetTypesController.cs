using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.AssetType;
using System.Runtime.CompilerServices;
using ERP.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ERP.API.Models.ExpenseGetReponse;
using ERP.API.Models.ExpenseTypeGetResponse;
using ERP.API.Models.AssetTypeGetResponse;

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
        public async Task<IActionResult> Get(string? searchValue = "", int pageNumber = 1, int pageSize = 10)
        {
            var query =  this._repository.Get().AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e =>
                    e.Name.Contains(searchValue) 
                 
                );
            }
            var totalCount = await query.CountAsync();

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var assetType = await query.ToListAsync();
            var result = assetType.Select(x => new AssetTypeGetResponseVM
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
            var paginationResult = new PaginatedResult<AssetTypeGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
            });
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var assetType = await this._repository.Get(id).Include(p => p.Assets).FirstOrDefaultAsync();
            if (assetType != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        Id = assetType.Id,
                        Name = assetType.Name,
                    }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AssetTypePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assetType = new AssetType
            {
                Name = model.Name,
            };

            _repository.Add(assetType);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    assetType.Id,
                    assetType.Name,
                }
            });
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AssetTypePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assetType = await this._repository.Get(id).SingleOrDefaultAsync();

            if (assetType != null)
            {
                assetType.Name = model.Name;

                this._repository.Update(assetType);
                await this._repository.SaveChanges();

                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                });
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var assetType = await this._repository.Get(id).SingleOrDefaultAsync();
            if (assetType != null)
            {
                assetType.IsActive = false;
                await this._repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                });
            }
            return NotFound();
        }
    }
}
