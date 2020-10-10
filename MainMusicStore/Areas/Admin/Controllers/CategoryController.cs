using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainMusicProject.DataAccess.IMainRepository;
using Microsoft.AspNetCore.Mvc;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        #region Variables
        private readonly IUnitOfWork _unitofwork; 
        #endregion

        #region Constractor
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofwork = unitOfWork;
        } 
        #endregion
        #region Actions
        public IActionResult Index()
        {
            return View();
        } 
        #endregion

        #region Api Calls
        public IActionResult GetAll()
        {
            var allObj = _unitofwork.category.GetAll();
            return Json(new { data = allObj });
        } 
        #endregion
    }
}