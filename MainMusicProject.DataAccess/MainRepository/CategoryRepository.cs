using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Data;
using MainMusicStore.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainMusicProject.DataAccess.MainRepository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Category category)
        {
            var data = _db.Categories.FirstOrDefault(x => x.Id == category.Id);
            if (data != null)
            {
                data.CategoryName = category.CategoryName;
            }
            
        }
    }
}
