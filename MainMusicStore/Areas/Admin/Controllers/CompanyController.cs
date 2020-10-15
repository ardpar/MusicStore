using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        #region Variables
        private readonly IUnitOfWork _uow;
        #endregion

        #region CTOR
        public CompanyController(IUnitOfWork uow)
        {
            _uow = uow;
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
            var allObj = _uow.company.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var deleteData = _uow.company.Get(id);
            if (deleteData == null)
                return Json(new { success = false, message = "Data Not Found!" });

            _uow.company.Remove(deleteData);
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
            Company cat = new Company();
            if (id == null)
            {
                //This for Create
                return View(cat);
            }

            cat = _uow.company.Get((int)id);
            if (cat != null)
            {
                return View(cat);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company Company)
        {
            if (ModelState.IsValid)
            {
                if (Company.Id == 0)
                {
                    //Create
                    _uow.company.Add(Company);
                }
                else
                {
                    //Update
                    _uow.company.Update(Company);
                }
                _uow.Save();
                return RedirectToAction("Index");
            }
            return View(Company);
        }
    }
}