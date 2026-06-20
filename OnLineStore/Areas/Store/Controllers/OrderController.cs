
using AspNetCoreHero.ToastNotification.Abstractions;

using AutoMapper;

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
        private readonly IMapper mapper;
        private readonly IShamCashService cashService;

        [BindProperty]
        public ShoppingCartViewModel? VM { get; set; }
        public OrderController(IUnitOfWork unitOfWork, IMailService service,
            IConfiguration configuration, INotyfService toastNotification, IOrderService orderService, IMapper mapper, IShamCashService cashService)
        {
            this.unitOfWork = unitOfWork;
            this.service = service;
            this.configuration = configuration;
            this.toastNotification = toastNotification;
            this.orderService = orderService;
            this.mapper = mapper;
            this.cashService = cashService;
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
            VM.OrderHeader.Payment = new PaymentViewModel()
            {
                Date = DateTime.Now,
                Status = StringDefault.PaymentPending,
                Amount = VM.OrderHeader.OrderTotal * 100,
                Currency = "SL, USD",
                CallbackUrl = configuration.GetValue<string>("Payment:CallbackUrl")!,
            };
            unitOfWork.OrderHeader.Add(mapper.Map<OrderHeader>(VM.OrderHeader));
            unitOfWork.Payment.Add(mapper.Map<Payment>(VM.OrderHeader.Payment));
            orderService.UpdateStatus(VM.OrderHeader.Id, StringDefault.OrderInProcess, StringDefault.PaymentPending);
            await unitOfWork.Save();

            // transform ShoppingCartLine to OrderItem
            foreach (var cart in VM.ListCart!)
            {
                OrderItemViewModel orderItem = new()
                {
                    OrderHeaderId = VM.OrderHeader.Id,
                    ProductId = cart.ProductId,
                    //ItemsPrice = cart.LinePrice,
                    Count = cart.Count
                };
                unitOfWork.OrderItem.Add(mapper.Map<OrderItem>(orderItem));
                await unitOfWork.Save();
            }
            return RedirectToAction("Pay", "Order", new { id = VM.OrderHeader.Id, area="Store" });
        }

        [HttpGet]
        public async Task<IActionResult> Pay(int id)
        {
            var payment = unitOfWork.Payment.GetFirstOrDefault(o => o.OrderHeaderId == id);
            if (payment == null)
            {
                var re = await cashService.InitiatePaymentAsync(payment!);
                // Update your order status in the database (e.g., mark as Paid)
                //orderService.UpdatePaymentInfo(id, payment!);
                unitOfWork.Payment.Add(re);
                await unitOfWork.Save();
            }
            return RedirectToAction(nameof(OrderConfirmation),"Order", new { id = id , area = "Store"});
        }
        [HttpGet("id")]
        public async Task<IActionResult> OrderConfirmation([FromQuery] int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var orderHeader = unitOfWork.OrderHeader.GetFirstOrDefault(o => o.Id == id, includeProperties: "User");
            var paymentFromDb = unitOfWork.Payment.GetFirstOrDefault(p => p.OrderHeaderId == id);
            if (orderHeader.OrderTotal == paymentFromDb.Amount / 100)
            {
                orderService.UpdatePaymentInfo(orderHeader.Id, paymentFromDb);
            }
            if (paymentFromDb.Id != orderHeader.PaymentId || paymentFromDb.Amount / 100 != orderHeader.OrderTotal)
            {
                return BadRequest();
            }
            if (paymentFromDb.Status == StringDefault.PaymentPaid)
            {
                return View(ApproveOP(orderHeader));
            }
            if (paymentFromDb.Status == StringDefault.PaymentRejected)
            {
                return View(RejectOP(orderHeader));
            }
            return View(new ResultViewModel()
            {
                Id = 0,
                Status = "مرفوض",
                Title = "لم يتم إتمام الطلب",
                Message = "عذراً .. حدث خطأ ما ولم يتم إتمام طلبك  نرجو إعادة المحاولة في وقت لاحق.."
            });
        }
        async Task<ResultViewModel> ApproveOP(OrderHeader orderHeader)
        {
            orderService.UpdateStatus(orderHeader.Id, StringDefault.OrderInProcess, StringDefault.PaymentPaid);
            await service.SendMailAsync(new MailData
            {
                ToId = orderHeader.User!.Email!,
                ToName = orderHeader.User.Name!,
                Subject = "طلب جديد",
                Body = "\\templates\\NewOrder.html",
                Order = orderHeader,
                //EmailAttachments = 
            });
            var shoppingCarts = unitOfWork.ShoppingCartLine.GetAll(u => u.UserNameIdentifier ==
                orderHeader.UserNameIdentifier).ToList();
            HttpContext.Session.Clear();
            unitOfWork.ShoppingCartLine.RemoveRange(shoppingCarts);
            await unitOfWork.Save();
            return new ResultViewModel()
            {
                Id = orderHeader.Id,
                Status = "مقبول",
                Title = "تم إستقبال الطلب بنجاح",
                Message = " شكراً لك لإتمام طلبك ..  لقد قمنا باستقباله  وسنقوم بإرسال رسالة التأكيد قريبا.. "
            };
        }

        async Task<ResultViewModel> RejectOP(OrderHeader orderHeader)
        {
            orderService.UpdateStatus(orderHeader.Id, StringDefault.OrderInProcess, StringDefault.PaymentRejected);
            await unitOfWork.Save();
            return new ResultViewModel()
            {
                Id = 0,
                Status = "مرفوض",
                Title = "لم يتم إتمام الطلب",
                Message = "عذراً .. حدث خطأ ما ولم يتم إتمام طلبك نرجو إعادة المحاولة في وقت لاحق.."
            };
        }


        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = unitOfWork.ShoppingCartLine.GetFirstOrDefault(c => c.Id == cartId);
            unitOfWork.ShoppingCartLine.IncrementCount(cart, 1);
            await unitOfWork.Save();
            return RedirectToAction(nameof(Index), "Order", new { area = "Store" });
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
            return RedirectToAction(nameof(Index), "Order", new { area = "Store" });
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            var cart = unitOfWork.ShoppingCartLine.GetFirstOrDefault(c => c.Id == cartId);
            unitOfWork.ShoppingCartLine.Remove(cart);
            await unitOfWork.Save();
            toastNotification.Information("لقد تم إزالة المنتج من سلة المشتريات");
            return RedirectToAction(nameof(Index), "Order", new { area = "Store" });
        }

        private ShoppingCartViewModel ShoppingCartOrder()
        {
            var id = User.Identities.FirstOrDefault()!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var shoppingCartLines = unitOfWork.ShoppingCartLine.GetAll(c => c.UserNameIdentifier == id, includeProperties: "Product");
            VM = new ShoppingCartViewModel()
            {
                ListCart = mapper.Map<IEnumerable<ShoppingCartLineViewModel>>(shoppingCartLines),
                OrderHeader = new(),
            };
            VM.OrderHeader.UserNameIdentifier = id;
            VM.OrderHeader.Payment = new();
            foreach (var cartLine in VM.ListCart)
            {
                cartLine.LinePrice = cartLine.Count * cartLine.Product!.Price;
                VM.OrderHeader.OrderTotal += cartLine.LinePrice;
            }
            return VM;
        }
    }
}
