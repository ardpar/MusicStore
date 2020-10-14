using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using MainMusicStore.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        #region Variables
        private readonly IUnitOfWork _uow;
        //wwwroota erişim özelliği sağlıyor
        private readonly IWebHostEnvironment _hostEnvironment;
        #endregion

        #region CTOR
        public ProductController(IUnitOfWork uow, IWebHostEnvironment hostEnvironment)
        {
            _uow = uow;
            _hostEnvironment = hostEnvironment;
        }
        #endregion

        #region Actions
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region API CALLS
        public IActionResult GetAll()
        {
            var allObj = _uow.product.GetAll(includeProperties:"Category");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deleteData = _uow.product.Get(id);
            if (deleteData == null)
                return Json(new { success = false, message = "Data Not Found!" });

            _uow.product.Remove(deleteData);
            _uow.Save();
            return Json(new { success = true, message = "Delete Operation Successfully" });
        }

        #endregion

        /// <summary>
        /// Create Or Update Get Method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _uow.category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _uow.cover.GetAll().Select(i => new SelectListItem
                {
                    Text =  i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                return View(productVM);
            }
            productVM.Product = _uow.product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                return NotFound();
            }
            return View(productVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {

                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = Path.Combine(webRootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    };
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    if (productVM.Product.Id != 0)
                    {
                        var productData = _uow.product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = productData.ImageUrl;
                    }
                }
                if (productVM.Product.Id == 0)
                {
                    //Create
                    _uow.product.Add(productVM.Product);
                }
                else
                {
                    //Update
                    _uow.product.Update(productVM.Product);
                }
                _uow.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _uow.category.GetAll().Select(a => new SelectListItem
                {
                    Text = a.CategoryName,
                    Value = a.Id.ToString()
                });

                productVM.CoverTypeList = _uow.cover.GetAll().Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.Id.ToString()
                });
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _uow.product.Get(productVM.Product.Id);
                }
            }
            return View(productVM.Product);
        }
    }
}