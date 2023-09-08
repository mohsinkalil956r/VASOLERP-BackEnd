using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Client;
using ERP.API.Models;
using System.Linq;
using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.AssettGetResponse;
using ERP.API.Models.ContactsGetResponseVM;
using ERP.API.Models.Contacts;
using ERP.API.Models.ClientContactVM;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsRepository _repository;
        private readonly IContactRepository _contact;

        public ClientsController(IClientsRepository repository, IContactRepository contact)
        {
            this._repository = repository;
            this._contact = contact;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.GetClientWithContact().Where(y => y.Client.IsActive);

            // Apply search filter if searchQuery is provided and not null or empty
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    p.Client.FirstName.Contains(searchQuery) ||
                    p.Client.LastName.Contains(searchQuery) ||
                    p.Contact.Email.Contains(searchQuery) ||
                    p.Contact.PhoneNumber.Contains(searchQuery) ||
                    p.Contact.Website.Contains(searchQuery) ||
                    p.Contact.Address.Contains(searchQuery)

                    );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var clients = await query.ToListAsync();

            var result = clients.Select(p => new ClientGetResponseVM
            {
                Id = p.Client.Id,
                FirstName = p.Client.FirstName,
                LastName = p.Client.LastName,
                Email = p.Contact.Email,
                PhoneNumber = p.Contact.PhoneNumber,
                Website = p.Contact.Website,
                Address = p.Contact.Address,

            }).ToList();

            var paginationResult = new PaginatedResult<ClientGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
            });
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = this._repository.GetClientWithContact().Where(contact => contact.Contact.ReferenceId == id && contact.Contact.Type == "Client").AsQueryable();

            var client = await query.Select(client=> new ClientContactVM
            {
                Id = client.Client.Id,
                FirstName = client.Client.FirstName,
                LastName = client.Client.LastName,
                contact = new ContactViewModel
                {
                    Email = client.Contact.Email,
                    PhoneNumber = client.Contact.PhoneNumber,
                    Website = client.Contact.Website,
                    Address = client.Contact.Address,
                },
            }).FirstOrDefaultAsync();

            if (client != null)
            {

                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = client
                };


                return Ok(apiResponse);
            }

            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientContactPostVM model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
               FirstName = model.FirstName,
               LastName = model.LastName,
            };

            _repository.Add(client);
            await _repository.SaveChanges();

            var contacts = new Contact
            {
                Type = "Client",
                ReferenceId = client.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Contact.Email,
                PhoneNumber = model.Contact.PhoneNumber,
                Website = model.Contact.Website,
                Address = model.Contact.Address,
            };

            _contact.Add(contacts);
            await _repository.SaveChanges();


            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    client.Id,
                    client.FirstName,
                    client.LastName,

                }
            });

        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClientContactPostVM model)
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


                this._repository.Update(client);
                await this._repository.SaveChanges();


                var contacts = await this._contact.Get().ToListAsync();

                foreach (var contact in contacts)
                {
                    if (contact != null && contact.Type == "Client" && contact.ReferenceId == id)
                    {
                        var updateContact = new ContactViewModel
                        {
                            Email = model.Contact.Email,
                            PhoneNumber = model.Contact.PhoneNumber,
                            Website = model.Contact.Website,
                            Address = model.Contact.Address
                        };
                        contact.Email = updateContact.Email;
                        contact.PhoneNumber = updateContact.PhoneNumber;
                        contact.Website = updateContact.Website;
                        contact.Address = updateContact.Address;

                        this._contact.Update(contact);
                    }
                }

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
            var clients = await this._repository.Get(id).Include(project => project.Projects).FirstOrDefaultAsync();

            var contacts = await this._contact.Get().Where(contact => contact.ReferenceId == id && contact.Type == "Client").ToListAsync();

            if (clients != null)
            {
                clients.IsActive = false;
                await this._repository.SaveChanges();


                foreach (var contact in contacts)
                {
                    contact.IsActive = false;
                    await this._contact.SaveChanges();

                }
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
