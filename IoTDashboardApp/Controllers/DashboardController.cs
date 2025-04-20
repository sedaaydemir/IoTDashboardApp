
using IoTDashboardApp.Hubs;
using IoTDashboardApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

//sadece sayfayı acsın, sıcaklık verısını tasımasın

namespace IoTDashboardApp.Controllers
{
    public class DashboardController : Controller
    {
        //private readonly IHubContext<DashboardHub> _hubContext;
        //private readonly ModbusService _modbusService;

        //public DashboardController(IHubContext<DashboardHub> hubContext, ModbusService modbusService)
        //{
        //    _hubContext = hubContext;
        //    _modbusService = modbusService;
        //}
        ////sayfa acıldıgında baglantı yapılsın
        //[HttpGet]
        //public async Task<IActionResult> TestConnection()
        //{
        //    bool isConnected = await _modbusService.ConnectAsync();
        //    if (isConnected)
        //    {
        //        return Ok("Bağlantı başarılı.");
        //    }
        //    else
        //    {
        //        return StatusCode(500, "Bağlantı hatası.");
        //    }
        //}


        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Index","Login");
            }

            //// Sıcaklık verisini al
            //float temperature = await _modbusService.ReadTemperatureAsync();
            //ViewBag.Temperature = temperature;

            return View();
        }


        ////test amaclı sabıt verıler gırıldı sonradan iptal edıldı
        //public async Task<IActionResult> SendValue()
        //{
        //    float temperature = await _modbusService.ReadTemperatureAsync();
        //    await _hubContext.Clients.All.SendAsync("ReceiveValue", $"{temperature} °C");
        //    return Ok();
        //}
    }
}
