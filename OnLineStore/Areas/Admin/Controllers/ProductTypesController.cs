using AspNetCoreHero.ToastNotification.Abstractions;

using AutoMapper;

using Core;

using Data;
using Data.IRepository;
using Data.Repository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using OnLineStore.ViewModels;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class ProductTypesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotyfService toastNotification;
        private readonly IMapper mapper;

        public ProductTypesController(IUnitOfWork unitOfWork, INotyfService toastNotification, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.toastNotification = toastNotification;
            this.mapper = mapper;
        }
        public IActionResult Index()
        {
            var productT = unitOfWork.ProductType.GetAll();
            List<ProductTypeViewModel> list = new();
            foreach (var item in productT)
            {
                list.Add(mapper.Map<ProductTypeViewModel>(item));
            }
            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProductTypeViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypeViewModel protype)
        {
            if (ModelState.IsValid)
            {
                var productType = mapper.Map<ProductType>(protype);
                var existingType = unitOfWork.ProductType.GetFirstOrDefault(p=>p.Type == protype.Type);
                if (existingType != null)
                {
                    ModelState.AddModelError("", "هذا النوع موجود بالفعل");
                    return View(protype);
                }
                unitOfWork.ProductType.Add(productType);
                await unitOfWork.Save();
                toastNotification.Success("لقد تم إضافة نوع المنتج");
                return RedirectToAction(nameof(Index), "ProductTypes", new {area="Admin"});
            }
            return View(protype);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var type = unitOfWork.ProductType.GetFirstOrDefault(p=>p.Id == id);
            if (type == null)
            {
                return NotFound();
            }
            return View(mapper.Map<ProductTypeViewModel>(type));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypeViewModel protype)
        {
            if (ModelState.IsValid)
            {
                var productType = mapper.Map<ProductType>(protype);
                unitOfWork.ProductType.Update(productType);
                await unitOfWork.Save();
                toastNotification.Success("لقد تم تعديل نوع المنتج");
                return RedirectToAction(nameof(Index), "ProductTypes", new { area = "Admin" });
            }
            return View(protype);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var type = unitOfWork.ProductType.GetFirstOrDefault(p => p.Id == id);
            if (type == null)
            {
                return NotFound();
            }
            return View(mapper.Map<ProductTypeViewModel>(type));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductTypeViewModel protype)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != protype.Id)
            {
                return NotFound();
            }
            var type = unitOfWork.ProductType.GetFirstOrDefault(p => p.Id == id);
            if (type == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                unitOfWork.ProductType.Remove(type);
                await unitOfWork.Save();
                toastNotification.Information("لقد تم حذف نوع المنتج");
                return RedirectToAction(nameof(Index), "ProductTypes", new { area = "Admin" });
            }
            return View(type);
        }
    }
}
