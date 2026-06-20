using Data;
using Core;

using AspNetCoreHero.ToastNotification.Abstractions;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class MessagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
