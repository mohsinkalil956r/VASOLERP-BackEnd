using ERP.API.Models;
using ERP.API.Models.Expense;
using ERP.API.Models.ExpenseGetReponse;
using ERP.API.Models.ExpenseTypeGetResponse;
using ERP.API.Models.PaymentModeGetResponse;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> Get(string? searchQuery="", int pageNumber=1,int pageSize=10)
        {
            var query = this._repository.Get()
                .Include(e => e.ExpenseType)
                .Include(p => p.PaymentMode)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(e =>
                    e.Description.Contains(searchQuery) ||
                    e.ExpenseDate.ToString().Contains(searchQuery) ||
                    e.Amount.ToString().Contains(searchQuery)
                );
            }
            var totalCount = await query.CountAsync();

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var expense = await query.ToListAsync();

            var result = expense.Select(p => new ExpenseGetResponseVM
            {
                Id = p.Id,
                ExpenseDate = p.ExpenseDate,
                Amount = p.Amount,
                Description = p.Description,
                ExpenseType = new ExpenseTypeGetResponseVM { Id = p.ExpenseTypeId, Name = p.ExpenseType.Name},
                PaymentMode = new PaymentModeGetResponseVM { Id = p.PaymentModeId, Name = p.PaymentMode.Name},
            }).ToList();

            var paginationResult = new PaginatedResult<ExpenseGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
            }); 
        }
      
        [HttpGet("{id}")]
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
                        ExpenseDate = expense.ExpenseDate,
                        Description = expense.Description,
                        Amount = expense.Amount,
                        ExpenseTypeId = expense.ExpenseTypeId,
                        PaymentModeId = expense.PaymentModeId
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

        [HttpPut("{id}")]
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
        [HttpDelete("{id}")]
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
