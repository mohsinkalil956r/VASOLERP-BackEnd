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
        public async Task<IEnumerable<object>> Get() {
            var expenseType = await this._repository.Get()
                .Include(e => e.Expenses).ToListAsync();
            var result = expenseType.Select(r => new
            {
                r.Id,
                r.Name,
                Expenses = r.Expenses.Select(ex => new { 
                    ex.Id, 
                    ex.ExpenseDate, 
                    ex.Description, 
                    ex.Amount })

            }).ToList();

            return result;
        }

        // GET api/<ValuesController>/5
        [HttpGet("id")]
        public async Task<IActionResult> Get(int id) {
            var expenseType = await this._repository.Get(id).FirstOrDefaultAsync();
            if(expenseType != null) {
                return Ok(expenseType);
            }
            return NotFound();
        }

        // GET: api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseTypePostVM model) {
            var expenseType = new ExpenseType
            {
                Name = model.Name
            };
            if (expenseType != null) {
                _repository.Add(expenseType);
                await _repository.SaveChanges();
                return Ok();
            }
            return NotFound();
            
        }

        // PUT api/<ValuesController>/5
        [HttpPut("id")]
        public async Task<IActionResult> Put(int id, [FromBody] ExpenseTypePutVM model) {
            var expenseType = await this._repository.Get(id).FirstOrDefaultAsync();
            if(expenseType != null) {
                expenseType.Name = model.Name;

                this._repository.Update(expenseType);
                await this._repository.SaveChanges();
                return Ok();
            }
            return NotFound();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id) {
            var expenseType = await this._repository.Get(id).FirstOrDefaultAsync();
            if (expenseType != null)
            {
                expenseType.IsActive = false;
                await this._repository.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
