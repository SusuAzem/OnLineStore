using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotyfService toastNotification;

        public MessagesController(IUnitOfWork unitOfWork, INotyfService toastNotification)
        {
            this.unitOfWork = unitOfWork;
            this.toastNotification = toastNotification;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var messages = unitOfWork.Message.GetAll().ToList();
            return Json(new { data = messages });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var message = unitOfWork.Message.GetFirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                toastNotification.Error("خطأ بعملية الحذف");
                return Json(new { success = false, message = "خطأ بعملية الحذف" });
            }

            unitOfWork.Message.Remove(message);
            unitOfWork.Save();
            toastNotification.Success("تم الحذف بنجاح");
            return Json(new { success = true, message = "تم الحذف بنجاح" });
        }

    }
}
