using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using MainMusicStore.Models.ViewModels;
using MainMusicStore.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace MainMusicStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CardController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public CardController(IUnitOfWork uow, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _uow = uow;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public ShoppingCardVM ShoppingCardVM { get; set; }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCardVM = new ShoppingCardVM()
            {
                OrderHeader = new OrderHeader(),
                ListCard = _uow.shoppingCard.GetAll(u=>u.ApplicationUserId==claims.Value, includeProperties:"Product")

            };

            ShoppingCardVM.OrderHeader.OrderTotal = 0;
            ShoppingCardVM.OrderHeader.ApplicationUser = _uow.applicationUser.GetFirstOrDefault(u => u.Id == claims.Value,
                includeProperties: "Company");

            foreach (var card in ShoppingCardVM.ListCard)
            {
                card.Price = ProjectConstant.GetPriceBaseOnQuantity(card.Count,card.Product.Price,card.Product.Price50, card.Product.Price100);
                ShoppingCardVM.OrderHeader.OrderTotal += (card.Price * card.Count);
                card.Product.Description = ProjectConstant.ConvertToRawHtml(card.Product.Description);

                if (card.Product.Description.Length > 50)
                {
                    card.Product.Description = card.Product.Description.Substring(0, 49) + "....";
                }
            }
            return View(ShoppingCardVM);
        }

        
    }
}