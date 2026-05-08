using _7Colors.ViewModels;

using AspNetCoreHero.ToastNotification.Abstractions;

using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Mvc;

namespace OnLineStore.Areas.ECommerce.Controllers
{  
    public class MessagesController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailService mailService;
        private readonly INotyfService toastNotification;

        public MessagesController(IUnitOfWork unitOfWork, IMailService mailService
            , INotyfService toastNotification)
        {
            this.unitOfWork = unitOfWork;
            this.mailService = mailService;
            this.toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
        }

        public IActionResult New()
        {
            var viewModel = new MessageViewModel() { TimeSend= DateTime.Now};

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageViewModel message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var ms = new Message()
                {
                    Email = message.Email,
                    Content = message.Content,
                    PhoneNumber = message.PhoneNumber,
                    Name = message.Name,
                    TimeSend = DateTime.Now,
                };
                await mailService.ReceiveEmailAsync(new MailData
                {
                    ToId = message.Email,
                    ToName = message.Name,
                    Subject = "استمارة عميل",
                    Body = message.Content + Environment.NewLine + message.PhoneNumber,
                });
                unitOfWork.Message.Add(ms);
                await unitOfWork.Save();
                toastNotification.Success("لقد تم استلام الرسالة");
                return Json(new { IsSuccess = "redirect", description = Url.Action("Home", "Index", new { id = message.Id }), message });
            }

        }                        
        
    }
}
