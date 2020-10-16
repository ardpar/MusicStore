using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Data;
using MainMusicStore.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MainMusicStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        #region Variables

        private readonly ApplicationDbContext _db;
        #endregion

        #region CTOR
        public UserController(ApplicationDbContext db)
        {
            _db = db;
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
            var userList = _db.ApplicationUsers.Include(c => c.Company).ToList();

            var userRole = _db.UserRoles.ToList();

            var roles = _db.Roles.ToList();

            foreach (var item in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == item.Id).RoleId;
                item.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

                if (item.Company == null)
                {
                    item.Company = new Company()
                    {
                        Name = string.Empty

                    };
                }
            }
            return Json(new { data = userList });
        }

        [HttpPost]
        public IActionResult LockUnLock([FromBody] string id)
        {
            var data = _db.ApplicationUsers.FirstOrDefault(x => x.Id == id);
            if (data == null)
            {
                return Json(new { success = false, message = "Error while Locking" });
            }

            if (data.LockoutEnabled != null && data.LockoutEnd > DateTime.Now)
            {
                data.LockoutEnd = DateTime.Now;
            }
            else
            {
                data.LockoutEnd = DateTime.Now.AddYears(10);
            }

            _db.SaveChanges();
            return Json(new { success = true, message = "Operation Successfully" });

        }

        #endregion

        
        
    }
}