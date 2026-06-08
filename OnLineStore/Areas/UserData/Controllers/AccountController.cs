using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Google;
using System.Security.Claims;
using OnLineStore.ViewModels;
using AutoMapper;
using AspNetCoreHero.ToastNotification.Abstractions;
using Data.IRepository;
using Business;
using Core;

namespace OnLineStore.Areas.UserData.Controllers
{
    [Area("UserData")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailService mailService;
        private readonly IMapper mapper;
        private readonly INotyfService toastNotification;

        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork,
             IMailService mailService, IMapper mapper, INotyfService toastNotification)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.mailService = mailService;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }


        [HttpGet]
        [Authorize]
        public IActionResult Account(string nameId)
        {
            var user = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == nameId);
            if (user == null)
            {
                return RedirectToAction("Login", "Account", new { area = "Store" });
            }
            var vm = mapper.Map<UserViewModel>(user);
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Account(UserViewModel user, string nameId)
        {
            var exuser = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == nameId);
            if (exuser==null)
            {
                return RedirectToAction("Login", "Account", new { area = "Store" });
            }
            if (!ModelState.IsValid)
            {
                toastNotification.Warning("لم يتم إكمال تسجيل المعلومات");
                return RedirectToAction("Account", "Account", new { area = "UserData" });
            }
            else
            {
                if (exuser.Email == user.Email && exuser.NameIdentifier == user.Nameidentifier)
                {
                    exuser.StreetAddress = user.StreetAddress!;
                    exuser.City = user.City!;
                    exuser.Neighborhood = user.Neighborhood!;
                    exuser.PostalCode = user.PostalCode;
                    exuser.Phone = user.Phone!;
                    
                    unitOfWork.User.Update(exuser);
                    await unitOfWork.Save();                                        
                    toastNotification.Success("لقد تم إكمال تسجيل المعلومات بنجاح");
                    await mailService.SendMailAsync(new MailData
                    {
                        ToId = user.Email!,
                        ToName = user.Name!,
                        Subject = "مرحباً بك",
                        Body = "\\templates\\RegCom.html",
                    });
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Store" });
        }
    }
}