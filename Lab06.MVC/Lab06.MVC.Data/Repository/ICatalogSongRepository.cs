using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Domain.RepositoryModel;

namespace Lab06.MVC.Data.Repository
{
    public interface ICatalogSongRepository
    {
        Task Remove(int catalogId, int songId);
        Task Insert(CatalogSong item);
        Task Update(CatalogSong item);

        Task Save();
    }
}
