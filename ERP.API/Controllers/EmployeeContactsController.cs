﻿using ERP.API.Models;
using ERP.API.Models.EmployeeContacts;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeContactsController : ControllerBase
    {
        private readonly IEmployeeContactRepository _repository;
        public EmployeeContactsController(IEmployeeContactRepository repository)
        {
            this._repository = repository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<APIResponse<Object>> Get()
        {
            var employeeContacts = await this._repository.Get()
                .Include(e => e.Employee).ToListAsync();
           
            var result = employeeContacts.Select(e => new
            {
                e.Id,
                e.Email,
                e.PhoneNumber,
                e.Website,
                e.Address,
                Employee = new { e.Employee.Id, e.Employee.FristName, e.Employee.LastName }

            }).ToList();

            return new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = result
            };
            
            
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<APIResponse<object>> Get(int id)
        {
            var employeeContact = await this._repository.Get(id).FirstOrDefaultAsync();
            if (employeeContact != null)
            {
                var model = new EmployeeContactGetVM
                {
                    Email = employeeContact.Email,
                    Website = employeeContact.Website,
                    Address = employeeContact.Address,
                    PhoneNumber = employeeContact.PhoneNumber,

                };
                this._repository.Update(employeeContact);
                await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = model
                };
            }
            return new APIResponse<object>
            {
                IsError = true,
                Message = "",

            };

        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] EmployeeContactPostVM model)
        {
            var employeeContact = new EmployeeContact
            {
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Website = model.Website,
                Address = model.Address,
                EmployeeId = model.EmployeeId,
            };

            _repository.Add(employeeContact);
            await _repository.SaveChanges();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<APIResponse<object>> Put(int id, [FromBody] EmployeeContactPutVM model)
        {
            var employeeContact = await this._repository.Get(id).FirstOrDefaultAsync();


            if (employeeContact != null)
            {
                
                employeeContact.Email = model.Email;
                employeeContact.PhoneNumber = model.PhoneNumber;
                employeeContact.Website = model.Website;
                employeeContact.Address = model.Address;
                employeeContact.EmployeeId = model.EmployeeId;

                this._repository.Update(employeeContact);
                await this._repository.SaveChanges();

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                };
            }

            return new APIResponse<object>
            {
                IsError = true,
                Message = "",
            };

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<APIResponse<object>> Delete(int id)
        {
            var employeeContact = await this._repository.Get(id).FirstOrDefaultAsync();

            if (employeeContact != null)
            {
                employeeContact.IsActive = false;

                return new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                };
            }

            return new APIResponse<object>
            {
                IsError = true,
                Message = "",
            };
        }

    }
}
