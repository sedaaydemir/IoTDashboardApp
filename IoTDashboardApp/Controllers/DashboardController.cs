
using IoTDashboardApp.DTOs;
using IoTDashboardApp.Hubs;
using IoTDashboardApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

//sadece sayfayı acsın, sıcaklık verısını tasımasın

namespace IoTDashboardApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {   
        private readonly IModbusService _modbusService;
       
        public DashboardController(IModbusService modbusService)
        {
            _modbusService = modbusService;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Index", "Login");
            }

            await _modbusService.ConnectAsync();

            return View();

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
            float basinc = await _modbusService.ReadFromBasinc();
            return Ok(basinc);
        }






        //buton ıle sevıye okuma
        [HttpGet("readLevel")]
        public async Task<IActionResult> ReadLevel()
        {

            float level =await _modbusService.ReadLevelAsync();
            return Ok(level);
        }



        ////buton ıle offset  degerını plc ye yazma
        [HttpPost("write-float")]
        public async Task<IActionResult> WriteFloat([FromBody] PlcDto dto)
        {
            try
            {
                bool result = await _modbusService.WriteLevelAsync(dto.value);  // int değeri servise gönder

                if (result)
                    return Ok(new { message = "PLC'ye başarıyla yazıldı." });
                else
                    return StatusCode(500, new { message = "PLC'ye yazılamadı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"HATA: {ex.Message} - {ex.InnerException?.Message}" });
            }
        }

    }
}
