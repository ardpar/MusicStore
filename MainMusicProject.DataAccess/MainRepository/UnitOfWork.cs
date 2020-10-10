using MainMusicProject.DataAccess.IMainRepository;
using MainMusicStore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainMusicProject.DataAccess.MainRepository
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            category = new CategoryRepository(db);
            sp_call = new SPCallRepository(_db);
        }
        public ICategoryRepository category { get; private set; }

        public ISPCallRepository sp_call { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
