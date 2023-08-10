using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models;
using ERP.API.Models.ProjectGetResponse;
using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.ExpenseGetReponse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _repository;
        public ProjectsController(IProjectRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query =  this._repository.Get().Include(c => c.Client.ClientContacts).Include(c => c.Status).AsQueryable();

            // Apply search filter if searchQuery is provided and not null or empty
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    p.Name.Contains(searchQuery) ||
                    p.Description.Contains(searchQuery)||
                    p.StartDate.ToString().Contains(searchQuery) ||
                    p.DeadLine.ToString().Contains(searchQuery)||
                    p.Budget.ToString().Contains(searchQuery)
                    );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var clients = await query.ToListAsync();

            var result = clients.Select(p => new ProjectGetResponseVM
            {
                Id=p.Id,
             Name=   p.Name,
              Description=  p.Description,
              StartDate=  p.StartDate,
              DeadLine=  p.DeadLine,
             Budget=   p.Budget,
               Client = new ClientGetResponseVM
               {
                FirstName=   p.Client.FirstName,
                   },
               }).ToList();

            var paginationResult = new PaginatedResult<ProjectGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var project = await this._repository.Get(id).FirstOrDefaultAsync();
            if (project != null)
            {
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }
            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectPostVM model)
        {
            if (ModelState.IsValid) {
                var project = new Project
                {
                    Budget = model.Budget,
                    ClientId = model.ClientId,
                    DeadLine = model.DeadLine,
                    Description = model.Description,
                    Name = model.Name,
                    StartDate = model.StartDate,
                    StatusId=model.StatusId,
                    ProjectEmployees = model.EmployeeIds.Select(x => new ProjectEmployee { EmployeeId = x }).ToList()
            };

            _repository.Add(project);
            await _repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }
            else
            {
                return NotFound();
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectPutVM model)
        {
            var project = await this._repository.Get(id).Include(p => p.ProjectEmployees).FirstOrDefaultAsync();

            if (project != null)
            {
                project.ProjectEmployees = model.EmployeeIds.Select(e => new ProjectEmployee { ProjectId = id, EmployeeId = e }).ToList();
                project.StartDate = model.StartDate;
                project.StatusId = model.StatusId;
                project.DeadLine = model.DeadLine;
                project.Description = model.Description; 
                project.Name = model.Name;
                project.Budget = model.Budget;
                project.ClientId = model.ClientId;
                project.StatusId = model.StatusId;
                this._repository.Update(project);
                await this._repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await this._repository.Get(id).Include(p => p.ProjectEmployees).FirstOrDefaultAsync();
            if (project != null)
            {
                project.IsActive = false;
                await this._repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }
            else
            {
               return NotFound();
            }
        }
    }
}
