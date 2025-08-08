using erp_api.Models;
using erp_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace erp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _customerService.GetAll());

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id) => Ok(await _customerService.GetById(id));

        [HttpPost]

        public async Task<IActionResult> Post([FromBody] CustomerDto dto) => Ok(await _customerService.Create(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CustomerDto dto)
        {
            await _customerService.Update(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _customerService.Delete(id);
            return NoContent();
        }
    }
}
