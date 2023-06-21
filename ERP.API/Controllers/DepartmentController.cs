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
        public async Task<APIResponse<object>> Get()
        {
            var departments = await this._repository.Get().ToListAsync();
                

            var departmentresult = departments.Select(p => new
            {
                p.Id,
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
        public async Task<APIResponse<object>> Get(int id)
        {
            var department = await this._repository.Get(id).FirstOrDefaultAsync();
            if (department != null)
            {
                var result = new DepartmentGetVM
                {
                    Name = department.Name,
                };


                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = result
                };

            }
            return new APIResponse<object>
            {
                IsError = false,
                Message = ""
            };
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
        public async Task<APIResponse<object>> Put(int id, [FromBody] DepartmentPutVM departments)
        {
            var department = await this._repository.Get(id).FirstOrDefaultAsync();

            if (department!= null)
            {
                
                department.Name = departments.Name;
                

                this._repository.Update(department);
                await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = department
                };
            }

            return new APIResponse<object>
            {
                IsError = false,
                Message = ""
            };

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<APIResponse<object>> Delete(int id)
        {
            var department = await this._repository.Get(id).FirstOrDefaultAsync();
            if (department != null)
            {
                department.IsActive = false;
                await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = department
                };
            }
            return new APIResponse<object>
            {
                IsError = false,
                Message = ""
            };
        }

    }
}

