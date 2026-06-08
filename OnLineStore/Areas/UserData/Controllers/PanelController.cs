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

        public IActionResult FrontPage(string nameId)
        {
            return View();
        }

    }
}
