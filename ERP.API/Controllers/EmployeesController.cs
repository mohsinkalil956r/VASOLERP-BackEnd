using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.EmployeeContacts;
using ERP.API.Models.Employees;
using ERP.API.Models;
using ERP.API.Models.EmployeeGetResponse;
using ERP.API.Models.EmployeeContactGetResponse;
using ERP.API.Models.ClientContactResponse;
using ERP.API.Models.AssettGetResponse;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        public EmployeesController(IEmployeeRepository repository)
        {
            this._repository = repository;
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.Get().Include(p => p.EmployeeContacts).AsQueryable();

            // Apply search filter if searchQuery is provided and not null or empty
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    p.FirstName.Contains(searchQuery) ||
                    p.LastName.Contains(searchQuery) ||
                    p.Salary.ToString().Contains(searchQuery) ||
                    p.DOB.ToString().Contains(searchQuery) ||
                    p.CNIC.Contains(searchQuery) ||
                    p.ContractDate.ToString().Contains(searchQuery)
                );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var employees = await query.ToListAsync();

            var result = employees.Select(p => new EmployeeGetResponseVM
            {
                Id = p.Id,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Contacts = p.EmployeeContacts.Select(e => new EmployeeContactGetResponseVM { Id = e.Id, Email = e.Email, PhoneNumber = e.PhoneNumber, Website = e.Website, Address = e.Address, }).ToList()
            }).ToList();


            var paginationResult = new PaginatedResult<EmployeeGetResponseVM>(result, totalCount);
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
            var employee = await this._repository.Get(id).Include(p=>p.EmployeeContacts).FirstOrDefaultAsync();
            if (employee != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        employee.Id,
                        employee.FirstName,
                        employee.LastName,
                        employee.Salary,
                        employee.DOB,
                        employee.CNIC,
                        employee.ContractDate,
                       employee.EmployeeContacts,
                       employee.Department,
                       employee.IsActive,
                     }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }
        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = new Employee
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Salary = model.Salary,
                DOB = model.DOB,
                CNIC = model.CNIC,
                ContractDate=model.ContractDate,
                DepartmentId=model.DepartmentId,
                EmployeeContacts = model.Contacts.Select(x => new EmployeeContact { Address = x.Address ,Website=x.Website,PhoneNumber=x.PhoneNumber,Email=x.Email}).ToList() // Initialize the EmployeeContacts collection
            };

            _repository.Add(employee);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.Salary,
                    employee.DOB,
                    employee.CNIC,
                    employee.ContractDate,
                    employee.DepartmentId,
                }
            });
        }
        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] EmployeePutVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await this._repository.Get(id).Include(e=>e.EmployeeContacts).SingleOrDefaultAsync();

            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Salary = model.Salary;
                employee.DOB = model.DOB;
                employee.CNIC = model.CNIC;
                employee.ContractDate = model.ContractDate;
                employee.DepartmentId = model.DepartmentId;

                var contactIds = model.Contacts.Select(x => x.Id).ToList();



                employee.EmployeeContacts.Where(x => contactIds.Contains(x.Id)).ToList().ForEach(contact =>
                {
                    var modelContact = model.Contacts.Where(x => x.Id == contact.Id).First();
                    contact.PhoneNumber = modelContact.PhoneNumber;
                    contact.Email = modelContact.Email;
                    contact.Address = modelContact.Address;
                    contact.Website = modelContact.Website;
                });
              
               

                this._repository.Update(employee);
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
            var employee = await this._repository.Get(id).SingleOrDefaultAsync();
            if (employee != null)
            {
                employee.IsActive = false;
               await this. _repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                });
            }
            return NotFound();
        }
    }
}
