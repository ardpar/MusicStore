using MainMusicStore.Models.DbModels;

namespace MainMusicProject.DataAccess.IMainRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
    }
}
