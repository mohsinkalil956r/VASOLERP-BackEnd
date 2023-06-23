using ERP.API.Models;
using ERP.API.Models.ExpenseType;
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

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get() {
            var expenseType = await this._repository.Get().ToListAsync();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = expenseType.Select(x => new
                {
                    id = x.Id,
                    Name = x.Name,
                })
            });
        }

        // GET api/<ValuesController>/5
        [HttpGet("id")]
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
        [HttpPut("id")]
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
        [HttpDelete("id")]
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
