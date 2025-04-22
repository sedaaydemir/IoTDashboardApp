using Humanizer;
using IoTDashboardApp.DTOs;
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


        //sıcaklık okuma
        [HttpGet("temperature")]
        public async Task<IActionResult> GetTemperature()
        {
            float temp = await _modbusService.ReadTemperatureAsync();
            return Ok(temp);
        }

        //buton ıle basınc degerı okuma
        [HttpGet("read")]
        public async Task<IActionResult> ReadData()
        {
            float basinc =await _modbusService.ReadFromBasinc();
            return Ok(basinc);
        }


        //buton ıle offset  degerını plc ye yazma
        [HttpPost("write-float")]
        public async Task<IActionResult> WriteFloat([FromBody] PlcDto dto)
        {
            bool result = await _modbusService.WriteFloatToPlcAsync("D5", dto.scala); // Adres ve değeri al

            if (result)
                return Ok("PLC'ye başarıyla yazıldı.");
            else
                return StatusCode(500, "PLC'ye yazma hatası.");
        }
    }
}
