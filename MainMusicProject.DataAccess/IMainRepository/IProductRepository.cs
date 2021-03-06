﻿using MainMusicStore.Models.DbModels;

namespace MainMusicProject.DataAccess.IMainRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product product);
    }
}
