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
            cover = new CoverTypeRepository(_db);
            product = new ProductRepository(_db);
            company = new CompanyRepository(_db);
            applicationUser = new ApplicationUserRepository(_db);
            shoppingCard = new ShoppingCardRepository(_db);
            orderDetails = new OrderDetailsRepository(_db);
            orderHeader = new OrderHeaderRepository(_db);

        }

        
        public ICategoryRepository category { get; private set; }

        public ISPCallRepository sp_call { get; private set; }

        public ICoverTypeRepository cover { get; private set; }

        public IProductRepository product { get; private set; }

        public ICompanyRepository company { get; private set; }

        public IApplicationUserRepository applicationUser { get; private set; }

        public IShoppingCardRepository shoppingCard { get; private set; }

        public IOrderDetailsRepository orderDetails { get; private set; }

        public IOrderHeaderRepository orderHeader { get; private set; }

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
