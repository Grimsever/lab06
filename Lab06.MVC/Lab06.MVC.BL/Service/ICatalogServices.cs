using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Domain.RepositoryModel;

namespace Lab06.MVC.BL.Service
{
    public interface ICatalogServices : IServices<Catalog>
    {
        Task AddAlbumToCatalog(int catalogId, int albumId);

        Task RemoveSongFromCatalog(int catalogId, int songId);

        Task AddSongToCatalog(int catalogId, int songId);
    }
}