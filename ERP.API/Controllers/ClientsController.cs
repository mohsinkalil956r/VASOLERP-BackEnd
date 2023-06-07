using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientsRepository _repository;
        public ClientController(IClientsRepository repository)
        {
            this._repository = repository;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IEnumerable<Object>> Get()
        {
            var clients = await this._repository.Get()
                .Include(p=> p.Projects)
                .Include(p=>p.ClientContacts)
                .ToListAsync();

            var result= clients.Select(p =>new
            {
                p.Id,
                p.Name,
                Projects = p.Projects.Select(e => new { e.Id, e.Name, e.StartDate,e.DeadLine,e.Status }),
                  ClientContacts = p.ClientContacts.Select(e => new { e.Id, e.Email, e.PhoneNumber, e.Website,e.Address })
            } ).ToList();
            return result;
        }

        // GET a    pi/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get( int id)
        {
            var clients = await this._repository.Get(id).FirstOrDefaultAsync();
            if (clients != null)
            {
                var model = new ClientGetVM
                {
                    Name = clients.Name,
                };


                return Ok(model);

            }
            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] ClientPostVM model)
        {
            var project = new Client
            {
              
                Name = model.Name,
              };

            _repository.Add(project);
            await _repository.SaveChanges();

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put( int id,  ClientPostVM model)
        {


            var clients = await this._repository.Get(id).FirstOrDefaultAsync();

            if (clients != null)
            {
                clients.Name= model.Name;
                _repository.SaveChanges();
            }
            return Ok(model);


        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clients = await this._repository.Get(id).Include(project => project.Projects).Include(clientContact => clientContact.ClientContacts).FirstOrDefaultAsync();
            if (clients != null)
            {
               clients.IsActive= false;
                _repository.SaveChanges();
            }
            return Ok();
        }
    }
}
