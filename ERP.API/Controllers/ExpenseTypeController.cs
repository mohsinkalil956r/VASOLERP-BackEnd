using ERP.API.Models;
using ERP.API.Models.ExpenseGetReponse;
using ERP.API.Models.ExpenseType;
using ERP.API.Models.ExpenseTypeGetResponse;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IExpenseTypeRepository _repository;
        public ExpenseTypeController(IExpenseTypeRepository repository)
        {
            this._repository = repository;
            
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? searchValue="", int pageNumber=1, int pageSize=10) {
            var query = this._repository
                .Get()
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e =>
                e.Name.Contains(searchValue));
            }
            var totalCount = await query.CountAsync();
            query = query.Skip((pageNumber-1)*pageSize).Take(pageSize);
            var expenseType = await query.ToListAsync();
            var result = expenseType.Select(e => new ExpenseTypeGetResponseVM
            {
                Id = e.Id,
                Name = e.Name
            }).ToList();
            var pagginatedResult = new PaginatedResult<ExpenseTypeGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = pagginatedResult
            }); ;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) {
            var expenseType = await this._repository.Get(id).FirstOrDefaultAsync();
            if(expenseType != null) {
                var apiResponse = new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        Id = expenseType.Id,
                        Name = expenseType.Name,
                    }
                };
                return Ok(apiResponse);
            }
            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseTypePostVM model) 
        {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var expenseType = new ExpenseType
            {
                Name = model.Name
            };
            _repository.Add(expenseType);
            await _repository.SaveChanges();
            return Ok(new APIResponse<object>
            {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        expenseType.Id,
                        expenseType.Name,
                    }
            }) ;  
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ExpenseTypePutVM model) {
            if(!ModelState.IsValid) {
                return NotFound(ModelState);
            }
            var expenseType = await this._repository.Get(id).SingleOrDefaultAsync();
            if(expenseType != null) {
                expenseType.Name = model.Name;

                this._repository.Update(expenseType);
                await this._repository.SaveChanges();
                return Ok(new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                });
            }
            return NotFound();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var expenseType = await this._repository.Get(id).SingleOrDefaultAsync();
            if (expenseType != null)
            {
                expenseType.IsActive = false;
                await this._repository.SaveChanges();
                return Ok(new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                });
            }
            return NotFound();
        }
    }
}
