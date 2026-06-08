using AspNetCoreHero.ToastNotification.Abstractions;

using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

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
            return new JsonResult(new { data = messages });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var message = unitOfWork.Message.GetFirstOrDefault(m => m.Id == id);
            if (message == null)
            {
                toastNotification.Error("خطأ بعملية الحذف");
                return BadRequest(new { success = false, message = "خطأ بعملية الحذف" });
            }
            unitOfWork.Message.Remove(message);
            unitOfWork.Save();
            toastNotification.Success("تم الحذف بنجاح");
            return Ok(new { success = true, message = "تم الحذف بنجاح" });
        }

    }
}
