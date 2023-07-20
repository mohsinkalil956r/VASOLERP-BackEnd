using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.EmployeeContacts;
using ERP.API.Models.Employees;
using ERP.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        public EmployeesController(IEmployeeRepository repository)
        {
            this._repository = repository;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employee = await this._repository.Get().ToListAsync();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = employee.Select(x => new
                {
                    x.Id,
                    x.FirstName,
                    x.LastName,
                    x.Salary,
                    x.DOB,
                    x.CNIC,
                })
            });
        }
        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await this._repository.Get(id).FirstOrDefaultAsync();
            if (employee != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        employee.Id,
                        employee.FirstName,
                        employee.LastName,
                        employee.Salary,
                        employee.DOB,
                        employee.CNIC,
                    }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }
        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Salary = model.Salary,
                DOB = model.DOB,
                CNIC = model.CNIC,
                EmployeeContacts = model.Contacts.Select(x => new EmployeeContact { Address = x.Address ,Website=x.Website,PhoneNumber=x.PhoneNumber,Email=x.Email}).ToList() // Initialize the EmployeeContacts collection
            };

            _repository.Add(employee);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.Salary,
                    employee.DOB,
                    employee.CNIC,
                }
            });
        }
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await this._repository.Get(id).SingleOrDefaultAsync();

            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Salary = model.Salary;
                employee.DOB = model.DOB;
                employee.CNIC = model.CNIC;
                this._repository.Update(employee);
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
            var employee = await this._repository.Get(id).SingleOrDefaultAsync();
            if (employee != null)
            {
                employee.IsActive = false;
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
