using AspNetCoreHero.ToastNotification.Abstractions;

using AutoMapper;

using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;

using OnLineStore.ViewModels;

using System.ComponentModel;
using System.Security.Claims;

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
            var products = unitOfWork.Product.GetAll(includeProperties: "ProductType");
            var productTypes = unitOfWork.ProductType.GetAll();
            var vm = new ProductListViewModel()
            {
                Products = mapper.Map<IEnumerable<ProductItemViewModel>>(products),
                Types = mapper.Map<IEnumerable<ProductTypeViewModel>>(productTypes)
            };
            return View(vm);
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
            ViewBag.Title = $"{p.Name}";
            ViewBag.Description = $"أفضل العروض لـ {p.Name}. {p.ProductType}";
            ViewBag.Keywords = $"{p.ProductType}, online shopping, {p.Name}, متجر الكتروني";
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
            return RedirectToAction(nameof(Index),"Home", new {area="Store"});
        }
        
        #region API CALLS
        //[HttpGet("api/products")]
        //public ActionResult GetAll()
        //{
        //    var objFromDb = unitOfWork.Product.GetAll(includeProperties: "ProductType");
        //    return Json(new { data = objFromDb });
        //}

        [HttpGet("api/products")]
        public JsonResult Data()
        {
            var products = unitOfWork.Product.GetAll(includeProperties: "ProductType")
                .Select(p =>
            new { id = p.Id, name = p.Name, img = p.Image, type = p.ProductType!.Type, price = p.Price }).ToList();
            return Json(new { data = products });
        }
        #endregion
        public IActionResult Info()
        {          
            return View();
        }
    }
}


