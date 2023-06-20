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
using ERP.DAL.Migrations;

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
        public async Task<APIResponse<object>> Get()
        {
            var clientContact = await this._repository.Get()
                .Include(p => p.Client).ToListAsync();

            var result = clientContact.Select(p => new
            {
                p.Id,
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
        public async Task<APIResponse<object>> Get(int id)
        {
            var clientcontact = await this._repository.Get(id).FirstOrDefaultAsync();
            if (clientcontact != null)
            {
                var result = new ClientContactGetVM
                {
                    PhoneNumber = clientcontact.PhoneNumber,
                    Address = clientcontact.Address,
                    Email = clientcontact.Email,
                    Website = clientcontact.Website,

                };

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = result
                };

            }
            return new APIResponse<object>
            {
                IsError = true,
                Message = ""
            };
        }

        [HttpPost]
        public async Task Post([FromBody] ClientContactPostVM clientcontacts)
        {
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
        }


        [HttpPut("{id}")]
        public async Task<APIResponse<object>> Put(int id, [FromBody] ClientContactPutVM clientcontacts)
        {
            var clientContact = await this._repository.Get(id).FirstOrDefaultAsync();

            if (clientContact != null)
            {
                clientContact.ClientId = clientcontacts.ClientId; 
                clientContact.PhoneNumber = clientcontacts.PhoneNumber;
                clientContact.Address = clientcontacts.Address;
                clientContact.Email = clientcontacts.Email;
                clientContact.Website = clientcontacts.Website;

                this._repository.Update(clientContact);
                await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = clientContact
                };
            }

            return new APIResponse<object>
            {
                IsError = true,
                Message = ""
            };
        }

        
        [HttpDelete("{id}")]
        public async Task<APIResponse<object>> Delete(int id)
            {
            var clientContact = await this._repository.Get(id).FirstOrDefaultAsync();
            if (clientContact != null)
            {
                clientContact.IsActive = false;
                await this._repository.SaveChanges();

                    return new APIResponse<object>
                    {
                        IsError = false,
                        Message = "",
                        data = clientContact
                    };
            }
            return new APIResponse<object>
            {
                IsError = true,
                Message = ""
           };
        }

    }
}


