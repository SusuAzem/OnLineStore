using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using Data.IRepository;

using Business;

namespace OnLineStore.Controllers.Components
{
    [ViewComponent(Name = "ShoppingCart")]
    public class ShoppingCartViewComponent : ViewComponent
    {

        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity!;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                if (HttpContext.Session.GetInt32(StringDefault.SessionCart) != null)
                {
                    return View(HttpContext.Session.GetInt32(StringDefault.SessionCart));
                }
                else
                {
                    HttpContext.Session.SetInt32(StringDefault.SessionCart,
                        _unitOfWork.ShoppingCartLine.GetAll(u => u.User!.NameIdentifier == claim.Value).ToList().Count);
                    return View(HttpContext.Session.GetInt32(StringDefault.SessionCart));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
        }
    }
}
