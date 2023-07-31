using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Client;
using ERP.API.Models;
using ERP.API.Models.Employees;
using System.Linq;

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
        public async Task<IActionResult> Get(string searchValue="", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.Get()
                .Include(p => p.Projects)
                .Include(p => p.ClientContacts).AsQueryable();

            // Apply search filter if searchValue is provided
            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(p =>
                    p.FirstName.Contains(searchValue) ||
                    p.LastName.Contains(searchValue)||
                    p.ClientContacts.Any(cc => cc.Email.Contains(searchValue)) ||
                    p.ClientContacts.Any(cc => cc.Address.Contains(searchValue)) ||
                    p.ClientContacts.Any(cc => cc.Website.Contains(searchValue)) ||
                    p.ClientContacts.Any(cc => cc.PhoneNumber.Contains(searchValue)) ||
                    p.ClientContacts.Any(cc => cc.Country.Contains(searchValue))
                );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var clients = await query.ToListAsync();

            var result = clients.Select(p => new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                ClientContacts = p.ClientContacts.Select(e => new { e.Id, e.Email, e.PhoneNumber, e.Website, e.Address, e.Country })
            }).ToList();

            return Ok( new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    SearchValue = searchValue,
                    Results = result
                }
            });
        }


        // GET a    pi/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var clients = await this._repository.Get(id).Include(p => p.ClientContacts).FirstOrDefaultAsync();
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

                ClientContacts = model.contacts.Select(x => new ClientContact { Email = x.Email, PhoneNumber = x.PhoneNumber, Website = x.Website, Address = x.Address, Country = x.Country }).ToList()
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

            var client = await this._repository.Get(id).Include(e => e.ClientContacts).SingleOrDefaultAsync();

            if (client != null)
            {
                client.FirstName = model.FirstName;
                client.LastName = model.LastName;   


                var contactIds = model.contacts.Select(x => x.Id).ToList();



                client.ClientContacts.Where(x => contactIds.Contains(x.Id)).ToList().ForEach(contact =>
                {
                    var modelContact = model.contacts.Where(x => x.Id == contact.Id).First();
                    contact.PhoneNumber = modelContact.PhoneNumber;
                    contact.Email = modelContact.Email;
                    contact.Address = modelContact.Address;
                    contact.Website = modelContact.Website;
                });
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
            var clients = await this._repository.Get(id).Include(project => project.Projects).Include(clientContact => clientContact.ClientContacts).FirstOrDefaultAsync();
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
