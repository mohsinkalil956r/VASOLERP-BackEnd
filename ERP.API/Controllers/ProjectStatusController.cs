using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    public class ProjectStatusController : ControllerBase
    {
        // GET: ProjectStatusController
        private readonly IProjectStatusRepository _ProjectStatusRepository;
        public ProjectStatusController(IProjectStatusRepository ProjectStatusRepository)
        {
            this._ProjectStatusRepository = ProjectStatusRepository;
        }

        // Get All Statuss <StatusController>
        [HttpGet]
        public async Task<IEnumerable<ProjectStatus>> Get()
        {
            return await this._ProjectStatusRepository.Get().ToListAsync();
        }
        // Get Status by Id<StatusController>

        [HttpGet("{id}")]
        public async Task<ProjectStatus> Get(int id)
        {
            return await _ProjectStatusRepository.Get(id).FirstOrDefaultAsync();
        }

        // Add new Status<StatusController>
        [HttpPost]
        public void Post([FromBody] string name)
        {
            this._ProjectStatusRepository.Add(new ProjectStatus { Name = name });
        }

        // UPDATE/PUT Status by Id<StatusController>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            this._ProjectStatusRepository.Update(new ProjectStatus { Id = id, Name = value });
        }

        // DELETE Status using Id <StatusController>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this._ProjectStatusRepository.Delete(id);
        }
    }
}