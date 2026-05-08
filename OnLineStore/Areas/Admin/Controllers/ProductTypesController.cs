using AspNetCoreHero.ToastNotification.Abstractions;

using Core;

using Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class ProductTypesController : Controller
    {
        private readonly AppDbContext context;
        private readonly INotyfService toastNotification;

        public ProductTypesController(AppDbContext context, INotyfService toastNotification)
        {
            this.context = context;
            this.toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View(context.ProductTypes.ToList());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType protype)
        {
            if (ModelState.IsValid)
            {
                context.ProductTypes.Add(protype);
                await context.SaveChangesAsync();
                toastNotification.Success("لقد تم إضافة نوع المنتج");
                return RedirectToAction(nameof(Index));
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
            var type = context.ProductTypes.Find(id);
            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductType protype)
        {
            if (ModelState.IsValid)
            {
                context.ProductTypes.Update(protype);
                await context.SaveChangesAsync();
                toastNotification.Success("لقد تم تعديل نوع المنتج");
                return RedirectToAction(nameof(Index));
            }
            return View(protype);
        }
        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var type = context.ProductTypes.Find(id);
            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var type = context.ProductTypes.Find(id);
            if (type == null)
            {
                return NotFound();
            }
            return View(type);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, ProductType protype)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id != protype.Id)
            {
                return NotFound();
            }
            var type = context.ProductTypes.Find(id);
            if (type == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                context.ProductTypes.Remove(type);
                await context.SaveChangesAsync();
                toastNotification.Information("لقد تم حذف نوع المنتج");
                return RedirectToAction(nameof(Index));
            }
            return View(type);
        }
        #region MyRegion

        #endregion
    }
}
