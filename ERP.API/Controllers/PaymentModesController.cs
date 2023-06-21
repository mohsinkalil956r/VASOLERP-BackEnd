using ERP.API.Models;
using ERP.API.Models.PaymentModes;
using ERP.API.Models.Projects;
using ERP.DAL.DB.Entities;
using ERP.DAL.Repositories.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<APIResponse<Object>> Get()
        {
            var paymentModes = await this._repository.Get().ToListAsync();

            var result = paymentModes.Select(p => new
            {
                p.Id,
                p.Name

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
        public async Task<IActionResult> Get(int id)
        {
            var paymentMode = await this._repository.Get(id).FirstOrDefaultAsync();

            if(paymentMode != null) 
            {
                var result = new
                {
                    paymentMode.Id,
                    paymentMode.Name,
                };
                return Ok(new APIResponse<object>
                {
                    IsError = false,
                    Message = "",
                    data = result
                });
            }
            return NotFound();
           
        }

        // POST api/<ValuesController>
        [HttpPost]
        public async Task Post([FromBody] PaymentModePostVM model)
        {
            var paymentMode = new PaymentMode
            {
                Name = model.Name
            };

            _repository.Add(paymentMode);
            await _repository.SaveChanges();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PaymentModePutVM model)
        {
            var paymentMode = await this._repository.Get(id).FirstOrDefaultAsync();

            if (paymentMode != null)
            {

                paymentMode.Name = model.Name;
                
                this._repository.Update(paymentMode);
                await this._repository.SaveChanges();

                return Ok();
            }

            return NotFound();

        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
