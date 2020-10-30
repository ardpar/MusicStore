using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MainMusicStore.Models;
using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MainMusicStore.Utility;

namespace MainMusicStore.Area.Customer.Contollers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _uow;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> ProductList = _uow.product.GetAll(includeProperties: "Category,CoverType");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var shoppingCount = _uow.shoppingCard.GetAll(a => a.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(ProjectConstant.shoppingCard, shoppingCount);
            }

            return View(ProductList);
        }

        public IActionResult Details(int id )
        {
            var product = _uow.product.GetFirstOrDefault(p => p.Id == id, includeProperties: "Category,CoverType");
            ShoppingCard shoppingCard = new ShoppingCard()
            {
                Product = product,
                ProductId = product.Id
            };
            return View(shoppingCard);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCard cartObj)
        {
            cartObj.Id = 0;
            if (ModelState.IsValid)
            {
                var claimIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObj.ApplicationUserId = claim.Value;

                ShoppingCard fromDb = _uow.shoppingCard.GetFirstOrDefault(s => s.ApplicationUserId == cartObj.ApplicationUserId
                && s.ProductId == cartObj.ProductId, includeProperties: "Product");
                if (fromDb == null)
                {
                    //insert
                    _uow.shoppingCard.Add(cartObj);
                }
                else
                {
                    //update
                    fromDb.Count += cartObj.Count;
                }
                _uow.Save();

                var shoppingCount = _uow.shoppingCard.GetAll(a => a.ApplicationUserId == cartObj.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32(ProjectConstant.shoppingCard,shoppingCount);

                return RedirectToAction("Index");
            }
            else
            {
                var product = _uow.product.GetFirstOrDefault(p => p.Id == cartObj.ProductId, includeProperties: "Category,CoverType");
                ShoppingCard shoppingCard = new ShoppingCard()
                {
                    Product = product,
                    ProductId = product.Id
                };
                return View(shoppingCard);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
