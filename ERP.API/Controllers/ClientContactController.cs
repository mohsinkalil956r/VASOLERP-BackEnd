using ERP.API.Models.Users;
using ERP.API.Models;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.ClientContacts;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using System.Net;
using ERP.API.Models.PaymentModes;

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

        [HttpPost]
        public async Task Post([FromBody] ClientContactPostVM clientcontacts)
        {
            var clientContact = new ClientContact
            {
                PhoneNumber = clientcontacts.PhoneNumber,
                Address = clientcontacts.Address,
                Email = clientcontacts.Email,
                Website = clientcontacts.Website,
            };

            _repository.Add(clientContact);
            await _repository.SaveChanges();
        }

        [HttpGet]
        public async Task<APIResponse<Object>> Get()
        {
            var clientContact = await this._repository.Get()
                .Include(p => p.Client).ToListAsync();

            var result = clientContact.Select(p => new
            {
                p.PhoneNumber,
                p.Email,
                p.Address,
                p.Website,
                p.ClientId,

            }).ToList();

            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = result
            };
        }





        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var clientcontact = await this._repository.Get(id).FirstOrDefaultAsync();

            if (clientcontact != null)
            {
                var result = new ClientContactGetVM
                {
                    PhoneNumber = clientcontact.PhoneNumber,
                    Email = clientcontact.Email,
                    Website = clientcontact.Website,
                    Address = clientcontact.Address,
                   

                    
                };
                return Ok(result);
            }
            return NotFound();

        }





        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ClientContactPutVM clientcontacts)
        {
            var clientContact = await this._repository.Get().FirstOrDefaultAsync();

            if (clientContact != null)
            {
                clientContact.PhoneNumber = clientcontacts.PhoneNumber;
                clientContact.Address = clientcontacts.Address;
                clientContact.Email = clientcontacts.Email;
                clientContact.Website = clientcontacts.Website;

                this._repository.Update(clientContact);
                await this._repository.SaveChanges();

                return Ok();
            }

            return NotFound();

        }

        
        [HttpDelete("{id}")]
        
        public async Task Delete(int id)
        {
            var clientContact = await this._repository.Get(id).Include(p => p.ClientId).FirstOrDefaultAsync();
            if (clientContact != null)
            {
                clientContact.IsActive = false;
            }
        }
    }
}


