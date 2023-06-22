using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    public class StatusController : ControllerBase
    {
        // GET: StatusController
        private readonly IStatusRepository _StatusRepository;
        public StatusController(IStatusRepository StatusRepository)
        {
            this._StatusRepository = StatusRepository;
        }

        // Get All Statuss <StatusController>
        [HttpGet]
        public async Task<IEnumerable<Status>> Get()
        {
            return await this._StatusRepository.Get().ToListAsync();
        }
        // Get Status by Id<StatusController>

        [HttpGet("{id}")]
        public async Task<Status> Get(int id)
        {
            return await _StatusRepository.Get(id).FirstOrDefaultAsync();
        }

        // Add new Status<StatusController>
        [HttpPost]
        public void Post([FromBody] string name)
        {
            this._StatusRepository.Add(new Status { Name = name });
        }

        // UPDATE/PUT Status by Id<StatusController>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
            this._StatusRepository.Update(new Status { Id = id, Name = value });
        }

        // DELETE Status using Id <StatusController>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            this._StatusRepository.Delete(id);
        }
    }
}