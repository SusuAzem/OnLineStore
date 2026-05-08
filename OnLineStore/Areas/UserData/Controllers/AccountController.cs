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
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleCallback), "Account") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallback()
        {
            // The user is authenticated by Google and signed in via cookies here
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Error", "Home");
        }


        [HttpGet]
        [Authorize]
        public IActionResult Account(string nameId)
        {
            var user = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == nameId);
            if (user == null)
            {
                return RedirectToAction(nameof(Login), "Account");
            }

            var vm = mapper.Map<UserViewModel>(user);
            if (!user!.Registered)
            {
                vm.Age = 3;                
                vm.Phone = "05";              
            }
            else
            {
                vm.Age = user.Age;
                vm.Phone = user.Phone;               
            }
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Account(UserViewModel user, string nameId)
        {
            var exuser = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == nameId);
            exuser ??= await UserHalfReg();

            if (ModelState.IsValid)
            {
                if (exuser.Email == user.Email && exuser.NameIdentifier == user.Nameidentifier)
                {
                    exuser.Registered = true;
                    //exuser.StreetAddress = user.StreetAddress!;
                    //exuser.City = user.City!;
                    //exuser.Neighborhood = user.Neighborhood!;
                    //exuser.PostalCode = user.PostalCode;
                    exuser.Age = user.Age;
                    //exuser.ParentEmail = user.ParentEmail!;
                    //exuser.ParentPhone = user.ParentPhone!;
                    exuser.Phone = user.Phone!;
                    foreach (var role in exuser.Roles)
                    {
                        User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Role", role));
                    }
                    User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Registered", "true"));
                    unitOfWork.User.Update(exuser);
                    await unitOfWork.Save();
                    await mailService.SendMailAsync(new MailData
                    {
                        ToId = user.Email!,
                        ToName = user.Name!,
                        Subject = "مرحباً بك",
                        Body = "\\templates\\RegCom.html",
                    });
                  
                    toastNotification.Success("لقد تم إكمال تسجيل المعلومات بنجاح");
                }
            }
            foreach (var role in exuser.Roles)
            {
                User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Role", role));
            }
            User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Registered", "false"));
            toastNotification.Warning("لم يتم إكمال تسجيل المعلومات");
            return RedirectToAction(nameof(Index), "Home");
        }

        public async Task Login(string returnUrl)
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                RedirectUri = Url.Action("GoogleResponse", new { returnUrl } )
            });
        }

        public async Task<IActionResult> GoogleResponse(string returnUrl)
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var email = result.Principal!.FindFirstValue(ClaimTypes.Email);
            if (email == StringDefault.AdminEmail1)
                return await AdminReg(email);

            var nameId = result.Principal!.FindFirstValue(ClaimTypes.NameIdentifier);
            if (nameId == null)
                return BadRequest();
            if (nameId != null)
            {
                var exuser = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == nameId);
                exuser ??= await UserHalfReg();
                foreach (var role in exuser.Roles)
                {
                    User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Role", role));
                }
                if (exuser.LockoutEnd > DateTimeOffset.Now)
                {
                    await HttpContext.SignOutAsync();
                    toastNotification.Custom("قد تم حجب المستخدم عن تسجيل الدخول للمنصة.. تواصل مع الإدارة لرفع الحجب", 10, "red");
                    return RedirectToLocal(returnUrl);
                }
                if (!exuser!.Registered)
                {
                    User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Registered", "false"));
                    toastNotification.Warning("الرجاء القيام بإكمال معلومات المستخدم");
                    return RedirectToAction(nameof(Account), new { nameId });
                }
                if (exuser!.Registered)
                {
                    User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Registered", "true"));
                    toastNotification.Success("لقد تم تسجيل الدخول");
                    return RedirectToLocal(returnUrl);
                }
            }
            return RedirectToLocal(returnUrl);           
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction(nameof(Index), "Home");
        }

        private async Task<User> UserHalfReg()
        {
            var user = new User(
                    nameidentifier: User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                    name: User.FindFirstValue(ClaimTypes.Name)!,
                    givenName: User.FindFirstValue(ClaimTypes.GivenName)!,
                    surname: User.FindFirstValue(ClaimTypes.Surname)!,
                    email: User.FindFirstValue(ClaimTypes.Email)!);
            user.Roles.Add(StringDefault.User);
            unitOfWork.User.Add(user);
            await mailService.SendMailAsync(new MailData
            {
                ToId = user.Email!,
                ToName = user.Name!,
                Subject = "مرحباً بك",
                Body = "\\templates\\Hello.html",
            });
            await unitOfWork.Save();
            return user;
        }

        private async Task<ActionResult> AdminReg(string? email)
        {
            var admin = unitOfWork.User.GetFirstOrDefault(u => u.Email == email);
            if (admin == null)
            {
                admin = new User(
                    nameidentifier: User.FindFirstValue(ClaimTypes.NameIdentifier)!,
                    name: User.FindFirstValue(ClaimTypes.Name)!,
                    givenName: User.FindFirstValue(ClaimTypes.GivenName)!,
                    surname: User.FindFirstValue(ClaimTypes.Surname)!,
                    email: User.FindFirstValue(ClaimTypes.Email)!)
                {
                    Registered = true,                   
                };
                admin.Roles.Add(StringDefault.Admin);
                unitOfWork.User.Add(admin);
                User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Registered", "true"));
                User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Role", StringDefault.Admin));
                await mailService.SendMailAsync(new MailData
                {
                    ToId = admin.Email!,
                    ToName = admin.Name!,
                    Subject = "مرحباً بالأدمن",
                    Body = "\\templates\\HelloAdmin.html",
                });
                await unitOfWork.Save();
                toastNotification.Success("لقد تم إكمال تسجيل معلومات المسؤول بنجاح");
                return RedirectToAction(nameof(Index), "Home");
            }
            User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Registered", "true"));
            User.Identities.FirstOrDefault()!.AddClaim(new Claim(type: "Role", StringDefault.Admin));
            return RedirectToAction(nameof(Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }


        #region validation
        [HttpPost]
        public JsonResult NotEqualEmail(string ParentEmail, string Email)
        {
            var valid = ParentEmail != Email;
            return Json(valid);
        }

        [HttpPost]
        public JsonResult NotEqualPhone(string ParentPhone, string Phone)
        {
            var valid = ParentPhone != Phone;
            return Json(valid);
        }

        #endregion
    }
}