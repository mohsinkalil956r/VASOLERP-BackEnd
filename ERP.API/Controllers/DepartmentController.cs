using ERP.API.Models;
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
        public async Task<APIResponse<Object>> Get()
        {
            var departments = await this._repository.Get().ToListAsync();
                

            var departmentresult = departments.Select(p => new
            {
                p.Name,

            }).ToList();

            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = departmentresult
            };
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var department = await this._repository.Get(id).FirstOrDefaultAsync();

            if (department != null)
            {
                var result = new DepatmentGetVM
                {
                    Name = department.Name,
                };
                return Ok(result);
            }
            return NotFound();

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] DepartmentPostVM department)
        {
            var departments = new Department
            {
                Name = department.Name,
               
            };

            _repository.Add(departments);
            await _repository.SaveChanges();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] DepartmentPutVM departments)
        {
            var department = await this._repository.Get(id).FirstOrDefaultAsync();

            if (department!= null)
            {
                
                department.Name = departments.Name;
                

                this._repository.Update(department);
                await this._repository.SaveChanges();

                return Ok();
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

