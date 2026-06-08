using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.CodeAnalysis;
using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using System.ComponentModel;
using Core;
using Data.IRepository;
using Business;
using OnLineStore.ViewModels;

namespace OnLineStore.Areas.Store.Controllers
{
    [Area("Store")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly IMapper mapper;
        private readonly INotyfService toastNotification;

        public HomeController(ILogger<HomeController> Logger, IUnitOfWork unitOfWork,
            IWebHostEnvironment hostEnvironment, IMapper mapper, INotyfService toastNotification)
        {
            logger = Logger;
            this.unitOfWork = unitOfWork;
            this.hostEnvironment = hostEnvironment;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }
        public IActionResult Index()
        {            
            var vm = new ProductListViewModel()
            {
                Products = unitOfWork.Product.GetAll(includeProperties: "ProductType"),
                Types = unitOfWork.ProductType.GetAll()
            };
            return View(vm);
        }

        [HttpGet]
        public JsonResult Data()
        {
            var products = unitOfWork.Product.GetAll(includeProperties: "ProductType")
                .Select(p =>
            new { id = p.Id, name = p.Name, img = p.Image, type = p.ProductType!.Type, price = p.Price }).ToList();
            return Json(new { data = products });
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var p = unitOfWork.Product.GetFirstOrDefault(u => u.Id == id,
               includeProperties: "ProductType");
            ShoppingCartLineViewModel cartObj = new()
            {
                Count = 1,
                ProductId = id,
                LinePrice = p.Price,
                Product = mapper.Map<ProductItemViewModel>(p),
            };
            return View(cartObj);
        }

        [HttpPost]
        [ActionName("Detail")]
        [Authorize]
        [Route("Store/Home/Detail")]
        public async Task<ActionResult> ProductDetail(ShoppingCartLineViewModel shoppingCart)
        {
            var claim = User.Identities.FirstOrDefault()!.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            shoppingCart.UserNameIdentifier = claim;

            ShoppingCartLine excart = unitOfWork.ShoppingCartLine.GetFirstOrDefault(
                u => u.UserNameIdentifier == claim && u.ProductId == shoppingCart.ProductId);
            if (ModelState.IsValid)
            {
                if (excart == null)
                {
                    excart = new()
                    {
                        UserNameIdentifier = claim,
                        ProductId = shoppingCart.ProductId,
                        Count = shoppingCart.Count,
                    };
                    unitOfWork.ShoppingCartLine.Add(excart);
                }
                else
                {
                    unitOfWork.ShoppingCartLine.IncrementCount(excart, shoppingCart.Count);
                }
                await unitOfWork.Save();
                HttpContext.Session.SetInt32(StringDefault.SessionCart,
                       unitOfWork.ShoppingCartLine.GetAll(u => u.UserNameIdentifier == claim).ToList().Count);                               
                toastNotification.Success("لقد تم إضافة المنتج إلى سلة مشترياتك");
            }
            return RedirectToAction(nameof(Index));
        }
        
        #region API CALLS
        [HttpGet("api/products")]
        public ActionResult GetAll()
        {
            var objFromDb = unitOfWork.Product.GetAll(includeProperties: "ProductType");
            return Json(new { data = objFromDb });
        }
        #endregion
        public IActionResult Info()
        {          
            return View();
        }
    }
}


