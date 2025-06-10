using IoTDashboardApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace IoTDashboardApp.Controllers
{
    public class LoginController : Controller
    {
        //projenın bu asamasında sabıt olacak, daha sonra dınamık hale cevırebılırım
        private const string USERNAME = "admin";
        private const string PASSWORD = "1234";

        [HttpGet]
        public IActionResult Index()
        {
			// Kullanıcı zaten giriş yaptıysa direkt Dashboard'a yönlendir
			if (HttpContext.Session.GetString("IsLoggedIn") == "true")
				return RedirectToAction("Index", "Dashboard");

			return View();
        }


        //form gonderıldıgınde calısacak
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (model.UserName == USERNAME && model.Password == PASSWORD)//modelden aldıgıyla kıyaslıyor
            {
                //giris basarılı
                HttpContext.Session.SetString("IsLoggedIn", "true");
                return RedirectToAction("Index","Dashboard");
            }

            ViewBag.Error = "Kullanıcı Adı veya Şifre Hatalı";
            return View(model);
        }
    }
}
