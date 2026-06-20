using OnLineStore.ViewModels;

using AspNetCoreHero.ToastNotification.Abstractions;

using AutoMapper;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHost;
        private readonly IMapper mapper;
        private readonly INotyfService toastNotification;

        public ProductsController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment,
            IMapper mapper, INotyfService toastNotification)
        {
            this.unitOfWork = unitOfWork;
            webHost = hostEnvironment;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            var products = unitOfWork.Product.GetAll(includeProperties: "ProductType");
            List<ProductItemViewModel> list = new();
            foreach (var item in products)
            {
                list.Add(mapper.Map<ProductItemViewModel>(item));
            }
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var product = new ProductViewModel()
            {
                Product = new ProductItemViewModel(),
                ProductTypeList = new SelectList(unitOfWork.ProductType.GetAll(), "Id", "Type"),
            };
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel pro, [FromForm(Name = "Product.Image")] IFormFile Image)
        {
            var types = new SelectList(unitOfWork.ProductType.GetAll(), "Id", "Type");
            var existedPro = unitOfWork.Product.GetFirstOrDefault(c => c.Name == pro.Product!.Name & c.Price == pro.Product!.Price);
            if (existedPro != null)
            {
                ViewBag.message = "هذا المنتج موجود مسبقاً";
                pro.ProductTypeList = types;
                return View(pro);
            }
            if (Image != null)
            {
                var ex = Image.FileName[Image.FileName.LastIndexOf('.')..];
                var fileName = $"img_{DateTime.Now:dd-MM-yy-HH-mm-ss}{ex}";
                var name = Path.Combine(webHost.WebRootPath + "/images/products/", fileName);
                await Image.CopyToAsync(new FileStream(name, FileMode.Create));
                pro.Product!.Image = "/images/products/" + fileName;
            }
            if (Image == null)
            {
                pro.Product!.Image = "/images/noimage.PNG";
            }
            if (ModelState.IsValid)
            {
                var p = mapper.Map<Product>(pro.Product);
                unitOfWork.Product.Add(p);
                await unitOfWork.Save();
                toastNotification.Success("لقد تم إضافة المنتج");
                return RedirectToAction(nameof(Index), "Products", new {area = "Admin"});
            }
            pro.ProductTypeList = types;
            return View(pro);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pro = unitOfWork.Product.GetFirstOrDefault(c => c.Id == id, "ProductType");
            if (pro == null)
            {
                return NotFound();
            }
            var vm = new ProductViewModel()
            {
                Product = mapper.Map<ProductItemViewModel>(pro),
                ProductTypeList = new SelectList(unitOfWork.ProductType.GetAll(), "Id", "Type"),
            };
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel pro, [FromForm(Name = "Product.Image")] IFormFile Image)
        {
            var oldpath = Path.Combine(webHost.WebRootPath + pro.Product!.Image);
            if (Image != null)
            {
                var ex = Image.FileName[Image.FileName.LastIndexOf('.')..];
                var fileName = $"img_{DateTime.Now:dd-MM-yy-HH-mm-ss}{ex}";
                var name = Path.Combine(webHost.WebRootPath + "/images/products/", fileName);
                await Image.CopyToAsync(new FileStream(name, FileMode.Create));
                pro.Product!.Image = "/images/products/" + fileName;                               
            }
            if (Image == null)
            {
                pro.Product!.Image = "/images/noimage.PNG";
            }
            if (ModelState.IsValid)
            {
                var p = mapper.Map<Product>(pro.Product);
                unitOfWork.Product.Update(p);
                await unitOfWork.Save();
                if (System.IO.File.Exists(oldpath))
                {
                    System.IO.File.Delete(oldpath);
                }
                toastNotification.Success("لقد تم تعديل المنتج");
                return RedirectToAction(nameof(Index), "Products", new {area="Admin"});
            }
            return View(pro);
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pro = unitOfWork.Product.GetFirstOrDefault(c => c.Id == id, "ProductType");
            if (pro == null)
            {
                return NotFound();
            }
            return View(mapper.Map<ProductItemViewModel>(pro));
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var pro = unitOfWork.Product.GetFirstOrDefault(c => c.Id == id, "ProductType");
            if (pro == null)
            {
                return NotFound();
            }
            return View(mapper.Map<ProductItemViewModel>(pro));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductItemViewModel pro)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != pro.Id)
            {
                return NotFound();
            }
            var p = unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
            if (p == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                unitOfWork.Product.Remove(p);
                await unitOfWork.Save();
                var fullPath = webHost.WebRootPath + p.Image;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                toastNotification.Information("لقد تم حذف المنتج");
                return RedirectToAction(nameof(Index), "Products", new { area = "Admin" });
            }
            return View(pro);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = unitOfWork.Product.GetAll(includeProperties: "ProductType");
            return Json(new { data = productList });
        }      
        #endregion
    }
}

