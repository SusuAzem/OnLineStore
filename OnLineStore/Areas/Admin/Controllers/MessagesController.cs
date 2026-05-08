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
        private readonly IUnitOfWork unitOfWork;
        private readonly INotyfService toastNotification;

        public MessagesController(IUnitOfWork unitOfWork, INotyfService toastNotification)
        {
            this.unitOfWork = unitOfWork;
            this.toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
