using ERP.API.Models;
using ERP.API.Models.Client;
using ERP.API.Models.DepartmentController;
using ERP.API.Models.DepartmentGetResponse;
using ERP.API.Models.ExpenseGetReponse;
using ERP.API.Models.PaymentModes;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<IActionResult> Get(string? searchQuery="", int pageNumber = 1, int pageSize = 10)
        {
            var departments = this._repository.Get().AsQueryable() ;

            // Apply search filter if searchQuery is provided

            if (!string.IsNullOrEmpty(searchQuery))
            {
                departments = departments.Where(p => p.Name.Contains(searchQuery));
            }

            // Get the total count of departments without pagination
            var totalCount = await departments.CountAsync();

            // Apply pagination

            departments = departments.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var department = await departments.ToListAsync();

            var result = department.Select(p => new DepartmentGetPresponseVM
            {
              Id=  p.Id,
              Name=  p.Name,
            }).ToList();
            var paginationResult = new PaginatedResult<DepartmentGetPresponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    searchQuery = searchQuery,
                    Results = result
                }
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

