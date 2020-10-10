using MainMusicStore.Models.DbModels;

namespace MainMusicProject.DataAccess.IMainRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category category);
    }
}
