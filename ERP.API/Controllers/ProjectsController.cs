using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;

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


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Object>> Get()
        {
            var projects = await this._repository.Get()
                .Include(p => p.Client)
                .Include(p => p.Status)
                .Include(p => p.Employees)
                .ToListAsync();

            var result = projects.Select(p => new
            {
                p.Name,
                p.Description,
                p.StartDate,
                p.DeadLine,
                Status = new { p.Status.Id, p.Status.Name },
                Client = new { p.Client.Id, p.Client.Name },
                p.Budget,
                Employees =  p.Employees.Select(e => new { e.Id, e.FristName, e.LastName })

            }).ToList();

            return result;
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async void Post([FromBody] ProjectPostVM model)
        {
            var project = new Project
            {
                Budget = model.Budget,
                ClientId = model.ClientId,
                DeadLine = model.DeadLine,
                Description = model.Description,
                Name = model.Name,
                StartDate = model.StartDate,
                ProjectEmployees = model.EmployeeIds.Select(x => new ProjectEmployee { EmployeeId = x }).ToList()
            };

            _repository.Add(project);
            await _repository.SaveChanges();
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
                project.ProjectStatusId = model.ProjectStatusId;
                project.DeadLine = model.DeadLine;
                project.Description = model.Description; 
                project.Name = model.Name;
                project.Budget = model.Budget;
                project.ClientId = model.ClientId;

                this._repository.Update(project);
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
