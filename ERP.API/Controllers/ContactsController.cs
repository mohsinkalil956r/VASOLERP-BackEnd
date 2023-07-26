using AutoMapper.Internal;
using ERP.API.Models;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IEmployeeContactRepository _employeeContact;

        private readonly IClientContactRepository _clientContact;
        public ContactsController(IEmployeeContactRepository employeeContact,
            IClientContactRepository clientContact)
        {
            this._employeeContact = employeeContact;
            this._clientContact = clientContact;
        }



        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var employeeContacts = await this._employeeContact.Get()
                .Include(e => e.Employee)
                .Select(c => new { Type = "Employee",
                 FirstName = c.Employee.FirstName,
                 LastName = c.Employee.LastName })
                .ToListAsync();

            var clientContacts = await this._clientContact.Get()
                .Include(e => e.Client)
                .Select(c => new { Type = "Client",
                 Name = c.Client.Name })
                .ToListAsync();

            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = employeeContacts.Concat(clientContacts)
                
            }
                     );
        }



    }
}
