﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _repository;
        public ProjectsController(IProjectRepository repository)
        {
            this._repository = repository;
        }


        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var projects = await this._repository.Get().Include(c=>c.Client.ClientContacts)
                
                .ToListAsync();

            var result = projects.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.StartDate,
                p.DeadLine,
                Client = new
                {
                    p.Client.FirstName,
                    Country = p.Client.ClientContacts.Select(c => c.Country).FirstOrDefault() // Get the first Country associated with the ClientContact

                },
                p.Budget,
               }).ToList();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = result,
            });
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var project = await this._repository.Get(id).FirstOrDefaultAsync();
            if (project != null)
            {
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }
            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectPostVM model)
        {
            if (ModelState.IsValid) {
                var project = new Project
                {
                    Budget = model.Budget,
                    ClientId = model.ClientId,
                    DeadLine = model.DeadLine,
                    Description = model.Description,
                    Name = model.Name,
                    StartDate = model.StartDate,

                    Clients = model.client.Select(x => new Client { FirstName = x.FirstName }).ToList(),
                ProjectEmployees = model.EmployeeIds.Select(x => new ProjectEmployee { EmployeeId = x }).ToList()
            };

            _repository.Add(project);
            await _repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }
            else
            {
                return NotFound();
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectPutVM model)
        {
            var project = await this._repository.Get(id).Include(p => p.ProjectEmployees).FirstOrDefaultAsync();

            if (project != null)
            {
                project.ProjectEmployees = model.EmployeeIds.Select(e => new ProjectEmployee { ProjectId = id, EmployeeId = e }).ToList();
                project.StartDate = model.StartDate;
                project.StatusId = model.StatusId;
                project.DeadLine = model.DeadLine;
                project.Description = model.Description; 
                project.Name = model.Name;
                project.Budget = model.Budget;
                project.ClientId = model.ClientId;

                this._repository.Update(project);
                await this._repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await this._repository.Get(id).Include(p => p.ProjectEmployees).FirstOrDefaultAsync();
            if (project != null)
            {
                project.IsActive = false;
                await this._repository.SaveChanges();
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = project,
                });
            }
            else
            {
               return NotFound();
            }
        }
    }
}
