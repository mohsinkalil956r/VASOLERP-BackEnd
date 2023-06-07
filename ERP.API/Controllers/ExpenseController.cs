using ERP.API.Models.Expense;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseRepository _repository;
        public ExpenseController(IExpenseRepository repository)
        {
            this._repository = repository;
        }
        [HttpGet]
        public async Task<IEnumerable<object>> Get()
        {
            return  await this._repository.Get().ToListAsync();
            
        }
        [HttpGet("id")]
        public async Task<IActionResult> Get(int id) { 
        var expense = await this._repository.Get(id).FirstOrDefaultAsync();
            if(expense != null) {
                return Ok(expense);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpensePostVM model) {
            var expense = new Expense
            {
                ExpenseDate = model.ExpenseDate,
                Description = model.Description,
                Amount = model.Amount,
                ExpenseTypeId = model.ExpenseTypeId,
                PaymentModeId = model.PaymentModeId
            };
            _repository.Add(expense);
            await this._repository.SaveChanges();
            return Ok();
        }
        [HttpPut("id")]
        public async Task<IActionResult> Put(int id,[FromBody] ExpensePutVM model) {
            var expense = await this._repository.Get(id).FirstOrDefaultAsync();
            if(expense != null)
            {
                expense.Amount = model.Amount;
                expense.ExpenseDate = model.ExpenseDate;
                expense.Description = model.Description;
                expense.ExpenseTypeId = model.ExpenseTypeId;
                expense.PaymentModeId = model.PaymentModeId;

                await this._repository.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {
        var expense = this._repository.Get(id).FirstOrDefault();
            if(expense != null)
            {
                this._repository.Delete(id);
                await this._repository.SaveChanges();
                return Ok();
            }
            return NotFound();
        }
    }
}
