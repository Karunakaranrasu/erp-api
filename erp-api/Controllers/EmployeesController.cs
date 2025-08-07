using erp_api.Models;
using erp_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace erp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _employeeService.GetAll());

        [HttpGet("{id}")]

        public async Task<IActionResult> GetById(int id) => Ok(await _employeeService.GetById(id));

        [HttpPost]

        public async Task<IActionResult> Post([FromBody] EmployeeDto dto) => Ok(await _employeeService.Create(dto));

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EmployeeDto dto)
        {
            await _employeeService.Update(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _employeeService.Delete(id);
            return NoContent();
        }
    }
}
