using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.Employees;
using ERP.API.Models;
using ERP.API.Models.EmployeeGetResponse;
using ERP.API.Models.EmployeeContactVM;
using ERP.API.Models.EmployeeContactGetId;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]  
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;
        private readonly IContactRepository _contact;
        public EmployeesController(IEmployeeRepository repository, IContactRepository contact)
        {
            this._repository = repository;
            this._contact = contact;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query = this._repository.GetEmployeeWithContact().Where(x => x.Employee.IsActive);

            // Apply search filter if searchQuery is provided and not null or empty
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(p =>
                    p.Employee.FirstName.Contains(searchQuery) ||
                    p.Employee.LastName.Contains(searchQuery) ||
                    p.Employee.Salary.ToString().Contains(searchQuery) ||
                    p.Employee.DOB.ToString().Contains(searchQuery) ||
                    p.Employee.CNIC.Contains(searchQuery) ||
                    p.Employee.ContractDate.ToString().Contains(searchQuery) ||
                    p.Contact.Email.Contains(searchQuery) ||
                    p.Contact.PhoneNumber.Contains(searchQuery) ||
                    p.Contact.Address.Contains(searchQuery)

                );
            }

            // Get the total count of items without pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var employees = await query.ToListAsync();

            var result = employees.Select(p => new EmployeeGetResponseVM
            {
                Id = p.Employee.Id,
                FirstName = p.Employee.FirstName,
                LastName = p.Employee.LastName,
                DOB = p.Employee.DOB,
                CNIC = p.Employee.CNIC,
                Salary = p.Employee.Salary,
                ContractDate = p.Employee.ContractDate,
                Email = p.Contact.Email,
                PhoneNumber = p.Contact.PhoneNumber,
                Address = p.Contact.Address,


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
            var query =  this._repository.GetEmployeeWithContact().Where(contact => contact.Contact.ReferenceId == id && contact.Contact.Type == "Employee").AsQueryable();
           
            var employee = await query.FirstOrDefaultAsync();

            if (employee != null)
            {

                var employeeData = new EmployeeContactVM
                {
                    FirstName = employee.Employee.FirstName,
                    LastName = employee.Employee.LastName,
                    DOB= employee.Employee.DOB,
                    CNIC = employee.Employee.CNIC,
                    Salary = employee.Employee.Salary,
                    ContractDate= employee.Employee.ContractDate,

                    Contacts = new EmployeeContactGetIdVM { Email = employee.Contact.Email, PhoneNumber = employee.Contact.PhoneNumber,
                        Address = employee.Contact.Address,
                    },
                    
                    
                };

                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new 
                    {
                        employeeData 
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
                DepartmentId =model.DepartmentId,
                
            };

            _repository.Add(employee);
            await _repository.SaveChanges();

            var contacts = new Contact
                {
                    Type = "Employee",
                    ReferenceId = employee.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Contact.Email,
                    PhoneNumber = model.Contact.PhoneNumber,
                    Address = model.Contact.Address
                };

            _contact.Add(contacts);
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

            var employee = await this._repository.Get(id).SingleOrDefaultAsync();

            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.Salary = model.Salary;
                employee.DOB = model.DOB;
                employee.CNIC = model.CNIC;
                employee.ContractDate = model.ContractDate;
                employee.DepartmentId = model.DepartmentId;


                this._repository.Update(employee);
                await this._repository.SaveChanges();


                var contacts = await this._contact.Get().ToListAsync();

                foreach (var contact in contacts)
                {
                    if (contact != null && contact.Type == "Employee" && contact.ReferenceId == id)
                    {
                        contact.FirstName = model.Contact.FirstName;
                        contact.LastName = model.Contact.LastName;
                        contact.Email = model.Contact.Email;
                        contact.PhoneNumber = model.Contact.PhoneNumber;
                        contact.Address = model.Contact.Address;

                        this._contact.Update(contact);

                    }
                }

                await this._repository.SaveChanges();

                //var contactIds = model.Contacts.Select(x => x.Id).ToList();

                //employee.EmployeeContacts.Where(x => contactIds.Contains(x.Id)).ToList().ForEach(contact =>
                //{
                //    var modelContact = model.Contacts.Where(x => x.Id == contact.Id).First();
                //    contact.PhoneNumber = modelContact.PhoneNumber;
                //    contact.Email = modelContact.Email;
                //    contact.Address = modelContact.Address;
                //    contact.Website = modelContact.Website;
                //});

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

            var contacts = await this._contact.Get().Where(contact => contact.ReferenceId == id && contact.Type == "Employee").ToListAsync();

            if (employee != null)
            {
                employee.IsActive = false;
                await this. _repository.SaveChanges();


                foreach (var contact in contacts)
                {
                    contact.IsActive = false;
                    await this._contact.SaveChanges();
                }

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
