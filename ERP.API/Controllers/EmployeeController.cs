using ERP.API.Models;
using ERP.API.Models.Employees;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        public EmployeeController(IEmployeeRepository repository)
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
                    Id = x.Id,
                    Name = x.FirstName,
                    LastName = x.LastName,
                    DOB = x.DOB,
                    CNIC = x.CNIC,
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
                        Id = employee.Id,
                        Name = employee.FirstName,
                        LastName = employee.LastName,
                        DOB = employee.DOB,
                        CNIC = employee.CNIC,
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
               DOB = model.DOB,
               CNIC= model.CNIC,
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
                    employee.DOB,
                    employee.CNIC,
                }
            });
        }
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]  EmployeePostVM model)
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








