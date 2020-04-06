using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab06.MVC.Data.Repository
{
    public interface IRepository<TEntity, in TArgument> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(TArgument id);
        Task Remove(TEntity item);
        Task Insert(TEntity item);
        Task Update(TEntity item);

        Task Save();
    }
}