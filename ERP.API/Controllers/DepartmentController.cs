using ERP.API.Models;
using ERP.API.Models.Client;
using ERP.API.Models.DepartmentController;
using ERP.API.Models.PaymentModes;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _repository;
        public DepartmentController(IDepartmentRepository repository)
        {
            this._repository = repository;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var departments = await this._repository.Get().ToListAsync();

            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = departments.Select(d => new
                {
                    Id = d.Id,
                    name = d.Name
                })
            });
        } 

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]

        public async Task<IActionResult> Get(int id)
            {
                var department = await this._repository.Get(id).FirstOrDefaultAsync();
                if (department != null)
                {
                    var apiresponse = new APIResponse<object>
                    {
                        IsError = false,
                        Message = "",
                        data = new
                        {
                            Id = department.Id,
                            name = department.Name
                        }
                    };
                    return Ok(apiresponse);
                }
                return BadRequest(ModelState);
            }
            
               


        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DepartmentPostVM department)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var departments = new Department
            {
                Name = department.Name,
               
            };
            _repository.Add(departments);
            await _repository.SaveChanges();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    departments.Id, 
                    departments.Name
                }
            });
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DepartmentPutVM departments)
        {
            if (!ModelState.IsValid) {
                return NotFound(ModelState);
            }
            var department = await this._repository.Get(id).SingleOrDefaultAsync();
            if (department!= null)
            {
                
                department.Name = departments.Name;
                this._repository.Update(department);
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
        public async Task<IActionResult> Delete(int id)
        {
            var department = await this._repository.Get(id).SingleOrDefaultAsync();
            if (department != null)
            {
                department.IsActive = false;
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

