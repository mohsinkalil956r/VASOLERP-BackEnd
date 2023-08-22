using AutoMapper.Internal;
using ERP.API.Models;
using ERP.API.Models.Contacts;
using ERP.API.Models.ContactsGetResponseVM;
using ERP.API.Models.ExpenseGetReponse;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _repository;

        public ContactsController(IContactRepository repository)
        {
            this._repository = repository;
        }



        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.Get()
                .AsQueryable();

            // Apply search filter if searchValue is provided and not null or empty
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    p.Type.Contains(searchQuery) ||
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

            var contacts = await query.ToListAsync();

            var result = contacts.Select(p => new ContactsGetResponseVM
            {
               Id = p.Id,
               Type = p.Type,
               ReferenceId = p.ReferenceId,
               Email = p.Email,
               PhoneNumber = p.PhoneNumber,
               Website = p.Website,
               Address = p.Address,
               Country = p.Country,

            }).ToList();


            var paginationResult = new PaginatedResult<ContactsGetResponseVM>(result, totalCount);

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
            var contacts = await this._repository.Get(id).FirstOrDefaultAsync();
            if (contacts != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        contacts.Type,
                        contacts.Email,
                        contacts.PhoneNumber,
                        contacts.Website,
                        contacts.Address,
                        contacts.Country,

                    }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }


        //POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactsPostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var contacts = new Contact
            {
                Type = model.Type,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Website = model.Website,
                Address = model.Address,
                Country = model.Country,
                ReferenceId = null,

            };

            _repository.Add(contacts);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    contacts.Type,
                    contacts.Email,
                    contacts.PhoneNumber,
                    contacts.Website,
                    contacts.Address,
                    contacts.Country,
                    contacts.ReferenceId,
                }
            });

        }


        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ContactsPutVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contacts = await this._repository.Get(id).SingleOrDefaultAsync();

            if (contacts != null)
            {
                contacts.Email = model.Email;
                contacts.PhoneNumber = model.PhoneNumber;
                contacts.Website = model.Website;
                contacts.Address = model.Address;
                contacts.Country = model.Country;


                this._repository.Update(contacts);
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
            var contacts = await this._repository.Get(id).SingleOrDefaultAsync();

            if (contacts != null && contacts.Type != "Client" && contacts.Type != "Employee")
            {
                contacts.IsActive = false;
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
