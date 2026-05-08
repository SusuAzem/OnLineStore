
using AspNetCoreHero.ToastNotification.Abstractions;

using Data.IRepository;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using NuGet.DependencyResolver;


namespace OnLineStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotyfService toastNotification;

        public UsersController(IUnitOfWork unitOfWork, INotyfService toastNotification)
        {
            this.unitOfWork = unitOfWork;
            this.toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            var userList = unitOfWork.User.GetAll(u=>!u.Roles.Contains("Admin")).Select(
                    u => new { name = u.Name, email = u.Email, phone = u.Phone, role = u.Roles.ToList() , 
                        id = u.NameIdentifier, lockoutEnd = u.LockoutEnd }).ToList();                            
            return Json(new { data = userList });
        }

        public async Task<IActionResult> Block(string id)
        {
            var teacher = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == id);
            if (teacher != null)
            {
                unitOfWork.User.Remove(teacher);
                await unitOfWork.Save();
                toastNotification.Information($"لقد تم حذف المستخدم {teacher.Name}");
                return RedirectToAction(nameof(Index));
            }
            toastNotification.Error("خطأ خلال عملية الحذف");
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> LockUnlock(string id)
        {
            var exuser = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == id);
            string text;
            if (exuser == null)
            {
                toastNotification.Error("خطأ خلال عملية الحجب");
                return RedirectToAction(nameof(Index));
            }
            if  (exuser.LockoutEnd > DateTime.Now)
            {
                //user is currently locked, we will unlock them
                exuser.LockoutEnd = DateTime.Now;
                text ="لقد تم تفعيل المستخدم ";
            }
            else
            {
                exuser.LockoutEnd = DateTime.Now.AddYears(1000);
                text ="لقد تم حجب المستخدم ";
            }
            unitOfWork.User.Update(exuser);
            await unitOfWork.Save();
            toastNotification.Success(text);
            return RedirectToAction(nameof(Index));
        }
    }
}
