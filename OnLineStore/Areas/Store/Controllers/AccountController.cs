using AspNetCoreHero.ToastNotification.Abstractions;

using AutoMapper;

using Business;

using Core;

using Data.IRepository;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

using OnLineStore.ViewModels;

using System.Security.Claims;

namespace OnLineStore.Areas.Store.Controllers
{
    [Area("Store")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailService mailService;
        private readonly IMapper mapper;
        private readonly INotyfService toastNotification;
        private readonly IUserService userService;

        public AccountController(ILogger<AccountController> logger, IUnitOfWork unitOfWork,
             IMailService mailService, IMapper mapper, INotyfService toastNotification, IUserService userService)
        {
            _logger = logger;
            this.unitOfWork = unitOfWork;
            this.mailService = mailService;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
            this.userService = userService;
        }
        public async Task Login(string returnUrl)
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse", new { returnUrl })
                });
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var nameId = result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier);
            if (nameId == null)
                return BadRequest();
            if (!result.Succeeded)
            {
                toastNotification.Error("فشل تسجيل الدخول");
                return RedirectToAction("Login", "Account", new { area = "Store" });
            }
            var claims = result.Principal!.Identities.FirstOrDefault()!.Claims.ToList();
            var user = await userService.HalfReg(claims);
            if (user.LockoutEnd > DateTimeOffset.Now)
            {
                await HttpContext.SignOutAsync();
                toastNotification.Custom("قد تم حجب المستخدم عن تسجيل الدخول للمنصة.. تواصل مع الإدارة لرفع الحجب", 10, "red");
                return RedirectToLocal(returnUrl);
            }
            toastNotification.Success("تم تسجيل الدخول بنجاح");
            return RedirectToLocal(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home", new { area = "Store" });
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home", new { area = "Store" });
        }
    }
}
