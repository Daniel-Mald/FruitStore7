using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FruitStore.Areas.Jirafa.Controllers
{
    [Area("Jirafa")]
    public class HomeController : Controller
    {
        // GET: HomeController

       
        public ActionResult Index()
        {
            return View();
        }

    }
}
