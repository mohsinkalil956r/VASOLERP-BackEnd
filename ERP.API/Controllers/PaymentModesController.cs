using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using ERP.API.Models.AssetType;
using System.Runtime.CompilerServices;
using ERP.API.Models;
using ERP.API.Models.PaymentModes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentModesController : ControllerBase
    {
        private readonly IPaymentModeRepository _repository;
        public PaymentModesController(IPaymentModeRepository repository)
        {
            this._repository = repository;
        }

        // GET: api/<ValuesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Done Refectoring
            var paymentMode = await this._repository.Get().ToListAsync();
            return Ok(new APIResponse<object>
            {
                IsError = false,
                Message = "",
                data = paymentMode.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name,
                })
            });
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var paymentMode = await this._repository.Get(id).SingleOrDefaultAsync();
            if (paymentMode != null)
            {
                var apiResponse = new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                    data = new
                    {
                        Id = paymentMode.Id,
                        Name = paymentMode.Name,
                    }
                };

                return Ok(apiResponse);
            }

            return NotFound();
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentModePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMode = new PaymentMode
            {
                Name = model.Name,
            };

            _repository.Add(paymentMode);
            await _repository.SaveChanges();

            return Ok(new APIResponse<Object>
            {
                IsError = false,
                Message = "",
                data = new
                {
                  
                    paymentMode.Name,
                }
            });
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PaymentModePostVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var paymentMode = await this._repository.Get(id).SingleOrDefaultAsync();

            if (paymentMode != null)
            {
                paymentMode.Name = model.Name;

                this._repository.Update(paymentMode);
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
            var paymentMode = await this._repository.Get(id).SingleOrDefaultAsync();
            if (paymentMode != null)
            {
                paymentMode.IsActive = false;
                return Ok(new APIResponse<Object>
                {
                    IsError = false,
                    Message = "",
                });
            }
            await this._repository.SaveChanges();
            return NotFound();
        }
    }
}
