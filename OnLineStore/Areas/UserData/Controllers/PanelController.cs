using Data.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.Areas.UserData.Controllers
{
    [Area("UserData")]
    [Authorize]
    public class PanelController : Controller
    {

        public PanelController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FrontPage()
        {
            return View();
        }

    }
}
