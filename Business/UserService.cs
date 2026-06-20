using Core;

using Data.IRepository;
using Data.Repository;

using MailKit;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Business
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMailService mailService;

        public UserService(IUnitOfWork unitOfWork, IMailService mailService)
        {
            this.unitOfWork = unitOfWork;
            this.mailService = mailService;
        }
        public async Task<User> HalfReg(List<Claim> claims)
        {
            var id = claims.Find(c => c.Type == ClaimTypes.NameIdentifier)!.Value;
            var u = unitOfWork.User.GetFirstOrDefault(u => u.NameIdentifier == id);
            if ( u != null) {
                claims.Add(new Claim(type: "Role", u.Role!));
                return u;
            }
            var user = new User(
                    nameidentifier: claims.Find(c => c.Type == ClaimTypes.NameIdentifier)!.Value,
                    name: claims.Find(c => c.Type == ClaimTypes.Name)!.Value,
                    surname: claims.Find(c => c.Type == ClaimTypes.Surname)!.Value,
                    email: claims.Find(c => c.Type == ClaimTypes.Email)!.Value,
                    date: DateOnly.FromDateTime(DateTime.Now)
            );
            var email = claims.Find(c => c.Type == ClaimTypes.Email)!.Value;
            if (email == StringDefault.AdminEmail)
            {
                user.Role = "Admin";
                claims.Add(new Claim(type: "Role", "Admin"));
                await mailService.SendMailAsync(new MailData
                {
                    ToId = user.Email!,
                    ToName = user.Name!,
                    Subject = "مرحباً بالأدمن",
                    Body = "\\templates\\HelloAdmin.html",
                });
            }
            else
            {
                claims.Add(new Claim(type: "Role", "Client"));
                await mailService.SendMailAsync(new MailData
                {
                    ToId = user.Email!,
                    ToName = user.Name!,
                    Subject = "مرحباً بك",
                    Body = "\\templates\\Hello.html",
                });
            }
            unitOfWork.User.Add(user);
            await unitOfWork.Save();
            return user;
        }
    }
}
