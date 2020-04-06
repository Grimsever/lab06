using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lab06.MVC.BL.Service
{
    public interface IServices<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task Add(T item);

        Task<T> GetById(int id);

        Task Update(int id, T item);

        Task Remove(int id);
    }
}