
using AspNetCoreHero.ToastNotification.Abstractions;

using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OnLineStore.ViewModels;

using System.Security.Claims;

namespace OnLineStore.Areas.Store.Controllers
{
    [Area("Store")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailService service;
        private readonly IConfiguration configuration;
        private readonly INotyfService toastNotification;
        private readonly IOrderService orderService;

        [BindProperty]
        public ShoppingCartViewModel? VM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IMailService service,
            IConfiguration configuration, INotyfService toastNotification, IOrderService orderService)
        {
            this.unitOfWork = unitOfWork;
            this.service = service;
            this.configuration = configuration;
            this.toastNotification = toastNotification;
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            return View(ShoppingCartOrder());
        }

        public IActionResult Summary()
        {
            return View(ShoppingCartOrder());
        }

        [HttpPost]
        [Authorize]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SummaryPOST()
        {
            var VM = ShoppingCartOrder();
            VM!.OrderHeader!.OrderDate = DateTime.Now;
            VM.OrderHeader.OrderStatus = StringDefault.OrderInProcess;

            unitOfWork.OrderHeader.Add(VM.OrderHeader);
            orderService.UpdateStatus(VM.OrderHeader.Id, StringDefault.OrderInProcess, StringDefault.PaymentPending);


            // transform ShoppingCartLine to OrderItem
            foreach (var cart in VM.ListCart!)
            {
                OrderItem orderItem = new()
                {
                    OrderHeaderId = VM.OrderHeader.Id,
                    ProductId = cart.ProductId,
                    //ItemsPrice = cart.LinePrice,
                    Count = cart.Count
                };
                unitOfWork.OrderItem.Add(orderItem);
                await unitOfWork.Save();
            }
            return RedirectToAction("Pay", "Order", new { id = VM.OrderHeader.Id });
        }

        [HttpGet]
        public IActionResult Pay(int id)
        {
            var order = unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id);
            return View(order);
        }

        public async Task<IActionResult> OrderConfirmation([FromQuery] int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var payment = new Payment();
            var orderHeader = unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id, includeProperties: "User");
            var paymentFromDb = unitOfWork.Payment.GetFirstOrDefault(p => p.OrderHeaderId == id);
            if (orderHeader.OrderTotal == payment.Amount / 100)
            {
                orderService.UpdatePaymentInfo(orderHeader.Id, payment);
            }
            //await unitOfWork.Save();
            if (payment.Id != orderHeader.PaymentId || payment.Amount / 100 != orderHeader.OrderTotal)
            {
                return BadRequest();
            }
            if (paymentFromDb.Status == StringDefault.PaymentPaid)
            {
                return await ApproveOP(orderHeader);
            }
            else if (paymentFromDb.Status == StringDefault.PaymentRejected)
            {
                return await RejectOP(orderHeader);
            }
            return View(new PaymentResultViewModel()
            {
                Id = 0,
                Status = "مرفوض",
                Title = "لم يتم إتمام الطلب",
                Message = "عذراً .. حدث خطأ ما ولم يتم إتمام طلبك  نرجو إعادة المحاولة في وقت لاحق.."
            });
        }
        async Task<IActionResult> ApproveOP(OrderHeader orderHeader)
        {
            orderService.UpdateStatus(orderHeader.Id, StringDefault.OrderInProcess, StringDefault.PaymentPaid);
            await service.SendMailAsync(new MailData
            {
                ToId = orderHeader.User!.Email!,
                ToName = orderHeader.User.Name!,
                Subject = "طلب جديد - الألوان السبعة",
                Body = "\\templates\\NewOrder.html",
                Order = orderHeader,
                //EmailAttachments = 
            });
            var shoppingCarts = unitOfWork.ShoppingCartLine.GetAll(u => u.UserNameIdentifier ==
                orderHeader.UserNameIdentifier).ToList();
            HttpContext.Session.Clear();
            unitOfWork.ShoppingCartLine.RemoveRange(shoppingCarts);
            await unitOfWork.Save();
            return View(new PaymentResultViewModel()
            {
                Id = orderHeader.Id,
                Status = "مقبول",
                Title = "تم إستقبال الطلب بنجاح",
                Message = " شكراً لك لإتمام طلبك ..  لقد قمنا باستقباله  وسنقوم بإرسال رسالة التأكيد قريبا.. "
            });
        }

        async Task<IActionResult> RejectOP(OrderHeader orderHeader)
        {
            orderService.UpdateStatus(orderHeader.Id, StringDefault.OrderInProcess, StringDefault.PaymentRejected);
            await unitOfWork.Save();
            return View(new PaymentResultViewModel()
            {
                Id = 0,
                Status = "مرفوض",
                Title = "لم يتم إتمام الطلب",
                Message = "عذراً .. حدث خطأ ما ولم يتم إتمام طلبك  نرجو إعادة المحاولة في وقت لاحق.."
            });
        }


        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = unitOfWork.ShoppingCartLine.GetFirstOrDefault(c => c.Id == cartId);
            unitOfWork.ShoppingCartLine.IncrementCount(cart, 1);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
            //return ViewComponent("ShoppingCart");
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            var cart = unitOfWork.ShoppingCartLine.GetFirstOrDefault(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                unitOfWork.ShoppingCartLine.Remove(cart);
            }
            else
            {
                unitOfWork.ShoppingCartLine.DecrementCount(cart, 1);
            }
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = unitOfWork.ShoppingCartLine.GetFirstOrDefault(c => c.Id == cartId);
            unitOfWork.ShoppingCartLine.Remove(cart);
            await unitOfWork.Save();
            toastNotification.Information("لقد تم إزالة المنتج من سلة المشتريات");
            return RedirectToAction(nameof(Index));
        }

        private ShoppingCartViewModel ShoppingCartOrder()
        {
            var id = User.Identities.FirstOrDefault()!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            VM = new ShoppingCartViewModel()
            {
                ListCart = unitOfWork.ShoppingCartLine.GetAll
                (c => c.UserNameIdentifier == id, includeProperties: "Product"),
                OrderHeader = new(),
            };
            VM.OrderHeader.UserNameIdentifier = id;
            foreach (var cartLine in VM.ListCart)
            {
                cartLine.LinePrice = cartLine.Count * cartLine.Product!.Price;
                VM.OrderHeader.OrderTotal += cartLine.LinePrice;
            }
            return VM;
        }
    }
}
