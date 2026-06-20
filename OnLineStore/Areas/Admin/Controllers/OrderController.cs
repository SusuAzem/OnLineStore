using AspNetCoreHero.ToastNotification.Abstractions;

using AutoMapper;

using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OnLineStore.ViewModels;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotyfService toastNotification;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        [BindProperty]
        public OrderViewModel? OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IConfiguration configuration, 
            INotyfService toastNotification, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.toastNotification = toastNotification;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Details(int orderId)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == orderId, includeProperties: "User");
            var items = _unitOfWork.OrderItem.GetAll(o => o.OrderHeaderId == orderId, includeProperties: "Product");
            OrderVM = new OrderViewModel()
            {
                OrderHeader =  mapper.Map<OrderHeaderViewModel>(orderHeader),
                OrderItems = mapper.Map<IEnumerable<OrderItemViewModel>>(items)
            };
            return View(OrderVM);
        }

        //public async Task<IActionResult> VoidPayment(int id)
        //{
        //    var o = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);
        //    //MoyasarService.ApiKey = configuration["Moyasar:Test:SecretKey"];
        //    if (o != null)
        //    {
        //        var payment = new Payment(o.PaymentId);
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
