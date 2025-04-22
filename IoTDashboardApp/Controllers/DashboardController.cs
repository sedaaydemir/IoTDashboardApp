
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

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("IsLoggedIn") != "true")
            {
                return RedirectToAction("Index","Login");
            }
            return View();
        }
    }
}
