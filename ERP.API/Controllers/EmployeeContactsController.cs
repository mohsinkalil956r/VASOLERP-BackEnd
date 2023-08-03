using ERP.API.Models;
using ERP.API.Models.EmployeeContacts;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Get()
        {
            var employeeContacts = await this._repository.Get()
                .Include(e => e.Employee).ToListAsync();

            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = employeeContacts.Select(x => new
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Website = x.Website,
                    Address = x.Address,

                })
            }
                    );
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
