﻿using ERP.API.Models.Projects;
using ERP.API.Models.StatusesVM;
using ERP.API.Models;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using ERP.API.Models.AssetTypeGetResponse;
using ERP.API.Models.StatusGetResponse;

namespace ERP.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly IStatusesRepository _repository;
        public StatusesController(IStatusesRepository repository)
        {
            this._repository = repository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get(string? searchQuery = "", int pageNumber = 1, int pageSize = 10)
        {
            var query =  this._repository.Get().AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(e =>
                    e.Name.Contains(searchQuery)

                );
            }
            var totalCount = await query.CountAsync();

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var status = await query.ToListAsync();
            var result = status.Select(x => new StatusGetResponseVM
            {
                Id = x.Id,
                Name = x.Name,
                IsProgress = x.IsProgress,
                Progress = x.Progress,

            }).ToList();
            var paginationResult = new PaginatedResult<StatusGetResponseVM>(result, totalCount);

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
            var status = await this._repository.Get(id).FirstOrDefaultAsync();
            if (status != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        Id = status.Id,
                        Name = status.Name,
                        IsProgress = status.IsProgress,
                        Progress = status.Progress,
                    }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProjectStatusesPostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = new Statuses
            {
                Name = model.Name,
                IsProgress = model.IsProgress,
                Progress = model.Progress,
            };

            _repository.Add(status);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                    status.Id,
                    status.Name,
                    status.IsProgress,
                    status.Progress
                }
            });
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProjectStatusesPutVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var status = await this._repository.Get(id).SingleOrDefaultAsync();

            if (status != null)
            {
                status.Name = model.Name;
                status.IsProgress = model.IsProgress;
                status.Progress = model.Progress;

                this._repository.Update(status);
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
            var status = await this._repository.Get(id).SingleOrDefaultAsync();
            if (status != null)
            {
                status.IsActive = false;
                await this._repository.SaveChanges();
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