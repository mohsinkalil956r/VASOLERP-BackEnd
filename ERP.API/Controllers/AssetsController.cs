using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Assets;
using ERP.API.Models;
using ERP.API.Models.AssetTypeGetResponse;
using ERP.API.Models.AssettGetResponse;

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

        //[HttpGet]
        //public async Task<APIResponse<Object>> Get()
        //{
        //    var assets = await this._repository.Get()
        //        .Include(a => a.AssetType).ToListAsync();

        //    var result = assets.Select(p => new
        //    {
        //        p.Id,
        //        p.Name,
        //        p.Description,
        //        p.PurchaseDate,
        //        p.PurchasePrice,
        //        AssetType = new { p.AssetType.Id, p.AssetType.Name },

        //    }).ToList();

        //    return new APIResponse<object>
        //    {
        //        IsError = false,
        //        Message = "",
        //        data = result
        //    };
        //}



        [HttpGet]
        public async Task<IActionResult> Get(string? searchValue = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.Get().Include(p => p.AssetType).AsQueryable();

            // Apply search filter if searchValue is provided and not null or empty
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchValue) ||
                    p.Description.Contains(searchValue) ||
                    p.PurchaseDate.ToString().Contains(searchValue) ||
                    p.PurchasePrice.ToString().Contains(searchValue)
                    );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var assets = await query.ToListAsync();

            var result = assets.Select(p => new AssetGetResponseVM
            {Id= p.Id, Name=p.Name,Description= p.Description,PurchaseDate= p.PurchaseDate, PurchasePrice=p.PurchasePrice,

                AssetType =  new AssetTypeGetResponseVM {Id= p.AssetTypeId, Name=p.AssetType.Name },
            }).ToList();
            var paginationResult = new PaginatedResult<AssetGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
            });
        }



        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<APIResponse<object>> Get(int id)
        {
            var asset = await this._repository.Get(id).FirstOrDefaultAsync();
            if (asset != null)
            {
                var result = new AssetGetVM
                {
                    id = asset.Id,
                    Name = asset.Name,
                    Description = asset.Description,
                    PurchaseDate = asset.PurchaseDate,
                    PurchasePrice = asset.PurchasePrice,
                    AssetTypeId = asset.AssetTypeId,

                };
                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = result
                };
            }

            return new APIResponse<object>
            {
                IsError = true,
                Message = "",

            };
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task< APIResponse<object>> Post([FromBody] AssetPostVM model)
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
            return new APIResponse<object>
            {
                IsError = true,
                Message = "",
                data=asset,

            };
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<APIResponse<object>> Put(int id, [FromBody] AssetPutVM model)
        {
            var asset = await this._repository.Get(id).FirstOrDefaultAsync();

            if (asset != null)
            {
                asset.Name = model.Name;
                asset.AssetTypeId = model.AssetTypeId;
                asset.Description = model.Description;
                asset.PurchasePrice = model.PurchasePrice;
                asset.PurchaseDate = model.PurchaseDate;

                await this._repository.SaveChanges();
                //this._repository.Update(asset);
                //await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data=asset,
                };
            }

            return new APIResponse<object>
            {
                IsError = true,
                Message = "",
            };

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<APIResponse<object>> Delete(int id)
        {
            var asset = await this._repository.Get(id).FirstOrDefaultAsync();
            if (asset != null)
            {
                asset.IsActive = false;
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
    }
}