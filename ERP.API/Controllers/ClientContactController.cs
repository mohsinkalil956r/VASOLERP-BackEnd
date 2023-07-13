using ERP.API.Models.Users;
using ERP.API.Models;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.ClientContacts;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using System.Net;
using ERP.API.Models.Client;

namespace ERP.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ClientContactController : ControllerBase
    {
        private readonly IClientContactRepository _repository;
        public ClientContactController(IClientContactRepository repository)
        {
            this._repository = repository;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var clientContact = await this._repository.Get().ToListAsync();

            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = clientContact.Select(x => new
                {
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    Email = x.Email,
                    Address = x.Address,
                    Website = x.Website,
                    ClientId = x.ClientId,
                })
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var clientcontact = await this._repository.Get(id).FirstOrDefaultAsync();
            if (clientcontact != null)
            {
                var apiResponse = new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        id = clientcontact.Id,
                        PhoneNumber = clientcontact.PhoneNumber,
                        Address = clientcontact.Address,
                        Email = clientcontact.Email,
                        Website = clientcontact.Website,
                        ClientId = clientcontact.ClientId,
                    }
                };
                return Ok(apiResponse);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ClientContactPostVM clientcontacts)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var clientContact = new ClientContact
            {
                ClientId = clientcontacts.ClientId,
                PhoneNumber = clientcontacts.PhoneNumber,
                Address = clientcontacts.Address,
                Email = clientcontacts.Email,
                Website = clientcontacts.Website,
            };

            _repository.Add(clientContact);
            await _repository.SaveChanges();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    clientcontacts.ClientId,
                    clientcontacts.PhoneNumber,
                    clientcontacts.Address,
                    clientcontacts.Email,
                    clientcontacts.Website,
                }
            });
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClientContactPutVM clientcontacts)
        {
            if (!ModelState.IsValid)
            {
                return NotFound(ModelState);
            }
            var clientContact = await this._repository.Get(id).SingleOrDefaultAsync();

            if (clientContact != null)
            {
                clientContact.ClientId = clientcontacts.ClientId;
                clientContact.PhoneNumber = clientcontacts.PhoneNumber;
                clientContact.Address = clientcontacts.Address;
                clientContact.Email = clientcontacts.Email;
                clientContact.Website = clientcontacts.Website;

                this._repository.Update(clientContact);
                await this._repository.SaveChanges();

                return Ok(new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                });
            }

            return NotFound();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clientContact = await this._repository.Get(id).SingleOrDefaultAsync();
            if (clientContact != null)
            {
                clientContact.IsActive = false;
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


