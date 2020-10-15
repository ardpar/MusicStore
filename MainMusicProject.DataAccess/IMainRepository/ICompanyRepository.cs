using MainMusicStore.Models.DbModels;

namespace MainMusicProject.DataAccess.IMainRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company category);
    }
}
