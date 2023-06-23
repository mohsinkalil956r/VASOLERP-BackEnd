using ERP.API.Models;
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
        public async Task<IActionResult> Get()
        {
            var expense = await this._repository.Get()
                .Include(e=>e.ExpenseType)
                .Include(p=>p.PaymentMode)
                .ToListAsync();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = expense.Select(x=> new
                {
                    id = x.Id,
                    expensedate = x.ExpenseDate,
                    description = x.Description,
                    amount = x.Amount,
                    ExpenseType = new {x.ExpenseType.Id, x.ExpenseType.Name},
                    PaymentMode = new {x.PaymentMode.Id, x.PaymentMode.Name},
                 })
            }) ;
            
        }
        [HttpGet("id")]
        public async Task<IActionResult> Get(int id) { 
        var expense = await this._repository.Get(id)
                .FirstOrDefaultAsync();
            if(expense != null) {
                var apiResponse = new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        expensedate = expense.ExpenseDate,
                        description = expense.Description,
                        amount = expense.Amount,
                        expensetypeId = expense.ExpenseTypeId,
                        paymentmodetypeid = expense.PaymentModeId
                    }
                };
                
                return Ok(apiResponse);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpensePostVM model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }          
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
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    expense.Id,
                    expense.ExpenseDate,
                    expense.Description,
                    expense.Amount,
                    expense.ExpenseTypeId,
                    expense.PaymentModeId
                }
            });
        }
        [HttpPut("id")]
        public async Task<IActionResult> Put(int id,[FromBody] ExpensePutVM model) {
            if (!ModelState.IsValid) { 
                return BadRequest(ModelState); 
            }
            
            var expense = await this._repository.Get(id).SingleOrDefaultAsync();
            if(expense != null)
            {
                expense.Amount = model.Amount;
                expense.ExpenseDate = model.ExpenseDate;
                expense.Description = model.Description;
                expense.ExpenseTypeId = model.ExpenseTypeId;
                expense.PaymentModeId = model.PaymentModeId;
                this._repository.Update(expense);
                await this._repository.SaveChanges();
                return Ok(
                    new APIResponse<object>
                    {
                        IsError = false,
                        Message = "",
                    }
                    );
            }
            return NotFound();
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id) {
            var expense = await this._repository.Get(id).SingleOrDefaultAsync();
            if(expense != null)
            {
                expense.IsActive = false;
                await this._repository.SaveChanges();
                return Ok(new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                }
                    );
            }
            return NotFound();
        }
    }
}
