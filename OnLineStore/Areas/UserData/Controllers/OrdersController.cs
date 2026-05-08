using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using static Azure.Core.HttpHeader;

namespace OnLineStore.Areas.UserData.Controllers
{
    [Area("UserData")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index(string nameId)
        {
            var orderHeaders = unitOfWork.OrderHeader.GetAll(o => o.UserNameIdentifier == nameId);
            var list = orderHeaders.Select(o => new
            {
                id = o.Id,
                name = o.User!.Name,
                orderStatus = o.OrderStatus,
                orderTotal = o.OrderTotal
            }).ToList();
            return View();
        }

    }
}
