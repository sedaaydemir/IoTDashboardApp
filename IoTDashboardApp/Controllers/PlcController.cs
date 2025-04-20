using IoTDashboardApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace IoTDashboardApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlcController : ControllerBase
    {
        private readonly ModbusService _modbusService;

        public PlcController(ModbusService modbusService)
        {
            _modbusService = modbusService;
        }

        [HttpGet("temperature")]
        public async Task<IActionResult> GetTemperature()
        {
            float temp = await _modbusService.ReadTemperatureAsync();
            return Ok(temp);
        }
    }
}
