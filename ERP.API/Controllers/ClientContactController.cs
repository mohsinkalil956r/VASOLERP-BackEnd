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
using System.Diagnostics.Metrics;
using ERP.API.Models.ClientContactResponse;
using ERP.API.Models.ClientGetResponse;
using ERP.API.Models.ExpenseGetReponse;
using ERP.API.Models.ExpenseTypeGetResponse;
using ERP.API.Models.PaymentModeGetResponse;

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
        public async Task<IActionResult> Get(string? searchValue = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.Get().AsQueryable();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(e =>
                    e.Email.Contains(searchValue) ||
                    e.PhoneNumber.ToString().Contains(searchValue) ||
                    e.Address.Contains(searchValue)||
                      e.Website.Contains(searchValue) ||
                    e.Country.Contains(searchValue) 
                );
            }
            var totalCount = await query.CountAsync();

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var expense = await query.ToListAsync();
            var result = expense.Select(p => new ClientContactGetResponseVM
            {
                Id = p.Id,
                PhoneNumber = p.PhoneNumber,
                Email = p.Email,
                Address = p.Address,
                Website = p.Website,
                Country = p.Country,
                //Client = new ClientGetResponseVM { FirstName = p.Client.FirstName, LastName = p.Client.LastName }
            }).ToList();

            var paginationResult = new PaginatedResult<ClientContactGetResponseVM>(result, totalCount);
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paginationResult
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
                        //ClientId = clientcontact.ClientId,
                        Country = clientcontact.Country
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
                //ClientId = clientcontacts.ClientId,
                PhoneNumber = clientcontacts.PhoneNumber,
                Address = clientcontacts.Address,
                Email = clientcontacts.Email,
                Website = clientcontacts.Website,
                Country = clientcontacts.Country
            };

            _repository.Add(clientContact);
            await _repository.SaveChanges();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                   // clientcontacts.ClientId,
                    clientcontacts.PhoneNumber,
                    clientcontacts.Address,
                    clientcontacts.Email,
                    clientcontacts.Website,
                    clientcontacts.Country
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
               // clientContact.ClientId = clientcontacts.ClientId;
                clientContact.PhoneNumber = clientcontacts.PhoneNumber;
                clientContact.Address = clientcontacts.Address;
                clientContact.Email = clientcontacts.Email;
                clientContact.Website = clientcontacts.Website;
                clientContact.Country = clientcontacts.Country;

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


