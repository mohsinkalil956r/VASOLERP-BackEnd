using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Client;
using ERP.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _repository;
        public ClientsController(IClientsRepository repository)
        {
            this._repository = repository;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<APIResponse<object>> Get()
        {
            var clients = await this._repository.Get()
                .Include(p=> p.Projects)
                .Include(p=>p.ClientContacts)
                .ToListAsync();

            var result= clients.Select(p =>new
            {
                p.Id,
                p.Name,
                Projects = p.Projects.Select(e => new { e.Id, e.Name, e.StartDate,e.DeadLine, }),
                  ClientContacts = p.ClientContacts.Select(e => new { e.Id, e.Email, e.PhoneNumber, e.Website,e.Address })
            } ).ToList();
            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = result
            };
        }

        // GET a    pi/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<APIResponse<object>> Get( int id)
        {
            var clients = await this._repository.Get(id).FirstOrDefaultAsync();
          
                var model = new ClientGetVM
                {
                    Name = clients.Name,
                };


                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = model,
                };

          
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<APIResponse<object>> Post([FromBody] ClientPostVM model)
        {
            var project = new Client
            {
              
                Name = model.Name,
              };

            _repository.Add(project);
            await _repository.SaveChanges();
            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data =project,
            };


        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<APIResponse<object>> Put( int id,  ClientPostVM model)
        {


            var clients = await this._repository.Get(id).FirstOrDefaultAsync();

            if (clients != null)
            {
                clients.Name= model.Name;
                _repository.SaveChanges();
            }
            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = clients,
            };


        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<APIResponse<object>> Delete(int id)
        {
            var clients = await this._repository.Get(id).Include(project => project.Projects).Include(clientContact => clientContact.ClientContacts).FirstOrDefaultAsync();
            if (clients != null)
            {
               clients.IsActive= false;
               await  this._repository.SaveChanges();
            }
            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
              
            };
        }
    }
}
