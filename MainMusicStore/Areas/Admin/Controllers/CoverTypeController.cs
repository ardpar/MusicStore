using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Models.DbModels;
using MainMusicStore.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        #region Variables
        private readonly IUnitOfWork _uow;
        #endregion

        #region CTOR
        public CoverTypeController(IUnitOfWork uow)
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
            //var allObj = _uow.cover.GetAll();
            var allCoverTypes = _uow.sp_call.List<CoverType>(ProjectConstant.Proc_CoverType_GetAll, null);
            return Json(new { data = allCoverTypes });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);

            var deleteData = _uow.sp_call.OneRecord<CoverType>(ProjectConstant.Proc_CoverType_Get, parameter);
            //var deleteData = _uow.cover.Get(Id);
            if (deleteData == null)
                return Json(new { success = false, message = "Data Not Found!" });

            _uow.sp_call.Execute(ProjectConstant.Proc_CoverType_Delete,parameter);
            _uow.Save();

            //_uow.cover.Remove(deleteData);
            
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
            CoverType covertype = new CoverType();
            if (id == null)
            {
                //This for Create
                return View(covertype);
            }
            var parameter = new DynamicParameters();
            parameter.Add("@Id",id);
            covertype = _uow.sp_call.OneRecord<CoverType>(ProjectConstant.Proc_CoverType_Get, parameter);
            //cat = _uow.cover.Get((int)Id);
            if (covertype != null)
            {
                return View(covertype);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType cover)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", cover.Name);
                if (cover.Id == 0)
                {
                    //Create
                    //_uow.cover.Add(cover);
                    _uow.sp_call.Execute(ProjectConstant.Proc_CoverType_Create, parameter);
                }
                else
                {
                    parameter.Add("@Id", cover.Id);
                    //Update
                    //_uow.cover.Update(cover);
                    _uow.sp_call.Execute(ProjectConstant.Proc_CoverType_Update, parameter);
                }
                _uow.Save();
                return RedirectToAction("Index");
            }
            return View(cover);
        }
    }
}