using ERP.API.Models;
using ERP.API.Models.ClientContactResponse;
using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.EmployeeContactGetResponse;
using ERP.API.Models.EmployeeContacts;
using ERP.API.Models.EmployeeGetResponse;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
///////////////////////////////////////////////////////////////////////
namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeContactsController : ControllerBase
    {
        private readonly IEmployeeContactRepository _repository;
        public EmployeeContactsController(IEmployeeContactRepository repository)
        {
            this._repository = repository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query =  this._repository.Get()
               .AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(e =>
                    e.Email.Contains(searchQuery) ||
                    e.PhoneNumber.ToString().Contains(searchQuery) ||
                    e.Address.Contains(searchQuery) ||
                      e.Website.Contains(searchQuery) 
                 
                );
            }
            var totalCount = await query.CountAsync();

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var expense = await query.ToListAsync();
            var result = expense.Select(p => new EmployeeContactGetResponseVM
            {
                Id = p.Id,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
                Website = p.Website,
               
                //Employee = new EmployeeGetResponseVM { FirstName = p.Employee.FirstName, LastName = p.Employee.LastName }
            }).ToList();

            var paginationResult = new PaginatedResult<EmployeeContactGetResponseVM>(result, totalCount);
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
            var employeeContact = await this._repository.Get(id).SingleOrDefaultAsync();
            if (employeeContact != null)
            {

                var apiResponse = new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        Email = employeeContact.Email,
                        Website = employeeContact.Website,
                        Address = employeeContact.Address,
                        PhoneNumber = employeeContact.PhoneNumber,
                        EmployeeId = employeeContact.Id
                    }
                };

                this._repository.Update(employeeContact);
                await this._repository.SaveChanges();

                return Ok(apiResponse);
            }
            return NotFound();

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeContactPostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employeeContact = new EmployeeContact
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Website = model.Website,
                Address = model.Address,
               
            };

            _repository.Add(employeeContact);
            await _repository.SaveChanges();

            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    employeeContact.Id,
                    employeeContact.Email,
                    employeeContact.PhoneNumber,
                    employeeContact.Website,
                    employeeContact.Address,
                    employeeContact.EmployeeId
                }
            }
                    );
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeeContactPutVM model)
        {
            var employeeContact = await this._repository.Get(id).SingleOrDefaultAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (employeeContact != null)
            {

                employeeContact.Email = model.Email;
                employeeContact.PhoneNumber = model.PhoneNumber;
                employeeContact.Website = model.Website;
                employeeContact.Address = model.Address;
              

                this._repository.Update(employeeContact);
                await this._repository.SaveChanges();

                return Ok(new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data= employeeContact
                });
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employeeContact = await this._repository.Get(id).SingleOrDefaultAsync();
            if (employeeContact != null)
            {
                employeeContact.IsActive = false;

                this._repository.Update(employeeContact);
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
