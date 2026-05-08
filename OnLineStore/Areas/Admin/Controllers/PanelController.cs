using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class PanelController : Controller
    {
                
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
