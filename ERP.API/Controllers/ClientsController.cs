using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Client;
using ERP.API.Models;
using ERP.API.Models.Employees;
using System.Linq;
using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.ClientContactResponse;
using ERP.API.Models.AssettGetResponse;

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
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query =   this._repository.Get().AsQueryable();

            // Apply search filter if searchQuery is provided and not null or empty
            if (!string.IsNullOrEmpty(searchQuery))
            {
                    query = query.Where(p =>
                    p.FirstName.Contains(searchQuery) ||
                    p.LastName.Contains(searchQuery) ||
                    p.Email.Contains(searchQuery) ||
                    p.PhoneNumber.Contains(searchQuery) ||
                    p.Website.Contains(searchQuery) ||
                    p.Address.Contains(searchQuery) ||
                    p.Country.Contains(searchQuery) 

                    );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var clients = await query.ToListAsync();

            var result = clients.Select(p => new ClientGetResponseVM
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                Website = p.Website,
                Address = p.Address,
                Country = p.Country,
                //contacts = p.ClientContacts.Select(e => new ClientContactGetResponseVM { Id = e.Id, Email = e.Email, PhoneNumber = e.PhoneNumber, Website = e.Website, Address = e.Address, Country = e.Country }).ToList()
            }).ToList();

            var paginationResult = new PaginatedResult<ClientGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
            });
        }


        // GET a    pi/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var clients = await this._repository.Get(id).FirstOrDefaultAsync();
            if (clients != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        clients.FirstName,
                        clients.LastName,
                        clients.Email,
                        clients.PhoneNumber,
                        clients.Website,
                        clients.Address,
                        clients.Country

                    }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientPostVM model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var clients = new Client
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
               Email = model.Email,
               PhoneNumber = model.PhoneNumber,
               Website = model.Website,
               Address = model.Address,
               Country = model.Country

                //ClientContacts = model.contacts.Select(x => new ClientContact { Email = x.Email, PhoneNumber = x.PhoneNumber, Website = x.Website, Address = x.Address, Country = x.Country }).ToList()
            };

            _repository.Add(clients);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    clients.Id,
                    clients.FirstName,
                    clients.LastName,
                    clients.Email,
                    clients.PhoneNumber,
                    clients.Website,
                    clients.Address,
                    clients.Country

                }
            });

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClientPutVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = await this._repository.Get(id).SingleOrDefaultAsync();

            if (client != null)
            {
                client.FirstName = model.FirstName;
                client.LastName = model.LastName;
                client.Email = model.Email;
                client.PhoneNumber = model.PhoneNumber;
                client.Website = model.Website;
                client.Address = model.Address;
                client.Country = model.Country;


                //var contactIds = model.contacts.Select(x => x.Id).ToList();



                //client.ClientContacts.Where(x => contactIds.Contains(x.Id)).ToList().ForEach(contact =>
                //{
                //    var modelContact = model.contacts.Where(x => x.Id == contact.Id).First();
                //    contact.PhoneNumber = modelContact.PhoneNumber;
                //    contact.Email = modelContact.Email;
                //    contact.Address = modelContact.Address;
                //    contact.Website = modelContact.Website;
                //});


                this._repository.Update(client);
                await this._repository.SaveChanges();

                return Ok(new APIResponse<Object>
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
            var clients = await this._repository.Get(id).Include(project => project.Projects).FirstOrDefaultAsync();
            if (clients != null)
            {
                clients.IsActive = false;
                await this._repository.SaveChanges();
            }
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",

            });
        }
    }
}
