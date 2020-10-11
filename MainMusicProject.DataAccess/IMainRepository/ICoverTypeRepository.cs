using MainMusicStore.Models.DbModels;

namespace MainMusicProject.DataAccess.IMainRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
