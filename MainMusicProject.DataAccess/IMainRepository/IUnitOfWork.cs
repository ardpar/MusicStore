using System;
using System.Collections.Generic;
using System.Text;

namespace MainMusicProject.DataAccess.IMainRepository
{
    public interface IUnitOfWork:IDisposable
    {
        ICategoryRepository category { get; }
        ISPCallRepository sp_call { get; }

        ICoverTypeRepository cover { get; }
        void Save();
    }
}
