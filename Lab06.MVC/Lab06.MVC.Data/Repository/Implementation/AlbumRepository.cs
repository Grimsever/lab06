using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.EntityFrameworkCore;

namespace Lab06.MVC.Data.Repository.Implementation
{
    internal class AlbumRepository : IRepository<Album, int>
    {
        private readonly MusicCatalogContext _context;

        public AlbumRepository(MusicCatalogContext context)
        {
            _context = context;
        }

        public async Task<Album> Get(int id)
        {
            var album = await _context.Albums
                .Include(x => x.Songs)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return album;
        }

        public async Task<IEnumerable<Album>> GetAll()
        {
            var albums = await _context.Albums
                .Include(x => x.Songs)
                .AsNoTracking()
                .ToListAsync();

            return albums;
        }

        public async Task Insert(Album item)
        {
            _context.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task Remove(Album album)
        {
            _context.Albums.Remove(album);

            await _context.SaveChangesAsync();
        }

        public async Task Update(Album item)
        {
            _context.Albums.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}