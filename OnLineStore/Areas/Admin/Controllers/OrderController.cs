using OnLineStore.ViewModels;

using AspNetCoreHero.ToastNotification.Abstractions;

using Business;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService toastNotification;
        private readonly IConfiguration configuration;

        [BindProperty]
        public OrderViewModel? OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IConfiguration configuration, 
            INotyfService toastNotification)
        {
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Details(int orderId)
        {
            OrderVM = new OrderViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId,
                              includeProperties: "User"),
                OrderItems = _unitOfWork.OrderItem.GetAll(o => o.OrderHeaderId == orderId,
                            includeProperties: "Product")
            };
            return View(OrderVM);
        }
     
        //public async Task<IActionResult> VoidPayment(int id)
        //{
        //    var o = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
        //    //MoyasarService.ApiKey = configuration["Moyasar:Test:SecretKey"];
        //    if (o != null)
        //    {
        //        var payment = Payment.Fetch(o.PaymentId);
        //        if (payment != null)
        //        {
        //            payment = payment.Refund(payment.Amount);
        //            _unitOfWork.OrderHeader.UpdatePaymentInfo(id, payment);
        //            _unitOfWork.OrderHeader.UpdateStatus(id, StringDefault.StatusRefunded, StringDefault.PaymentStatusRefunded);
        //            await _unitOfWork.Save();
        //            toastNotification.Success($"لقد تم إلغاء الدفع للطلب {o.Id}");
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
