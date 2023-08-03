using AutoMapper.Internal;
using ERP.API.Models;
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
        private readonly IEmployeeContactRepository _employeeContact;

        private readonly IClientContactRepository _clientContact;
        public ContactsController(IEmployeeContactRepository employeeContact,
            IClientContactRepository clientContact)
        {
            this._employeeContact = employeeContact;
            this._clientContact = clientContact;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string? searchValue = "", int pageNumber = 1, int pageSize = 10)
        {
            var employeeContacts = await this._employeeContact.Get()
                .Include(e => e.Employee)
                .Select(e => new
                {
                    Type = "Employee",
                    Id = e.EmployeeId, // Assuming EmployeeId is the unique identifier for employees
                    FirstName = e.Employee.FirstName,
                    LastName = e.Employee.LastName,
                })
                .ToListAsync();

            var clientContacts = await this._clientContact.Get()
                .Include(c => c.Client)
                .Select(c => new
                {
                    Type = "Client",
                    Id = c.ClientId, // Assuming ClientId is the unique identifier for clients
                    FirstName = c.Client.FirstName,
                    LastName = c.Client.LastName
                })
                .ToListAsync();

            // Concatenate employeeContacts and clientContacts into a single list
            var mergeLists = employeeContacts.Concat(clientContacts).ToList();

            // Apply search filter if searchValue is provided and not null or empty
            if (!string.IsNullOrEmpty(searchValue))
            {
                mergeLists = mergeLists.Where(p =>
                    p.Id.ToString().Contains(searchValue) || // Assuming Id is an integer; convert to string for search
                    p.FirstName.Contains(searchValue) ||
                    p.LastName.Contains(searchValue)
                ).ToList();
            }

            // Get the total count of items without pagination
            var totalCount = mergeLists.Count;

            // Apply pagination
            mergeLists = mergeLists.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            var result = mergeLists.Select(p => new
            {
                p.Id,
                p.FirstName,
                p.LastName,
                p.Type // Include the Type in the result
            }).ToList();

            return Ok(new APIResponse<object>
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



    }
}
