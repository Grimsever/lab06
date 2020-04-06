using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.EntityFrameworkCore;

namespace Lab06.MVC.Data.Repository.Implementation
{
    internal class CatalogRepository : IRepository<Catalog, int>
    {
        private readonly MusicCatalogContext _context;

        public CatalogRepository(MusicCatalogContext context)
        {
            _context = context;
        }

        public async Task<Catalog> Get(int id)
        {
            var album = await _context.Catalogs
                .Include(x => x.CatalogSong)
                .ThenInclude(x => x.Song)
                .ThenInclude(x => x.Album)
                .Include(x => x.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return album;
        }

        public async Task<IEnumerable<Catalog>> GetAll()
        {
            return await _context.Catalogs
                .Include(x => x.CatalogSong)
                .ThenInclude(x => x.Song)
                .ThenInclude(x => x.Album)
                .Include(x => x.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task Insert(Catalog item)
        {
            _context.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task Remove(Catalog item)
        {
            _context.Catalogs.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task Update(Catalog item)
        {

            _context.Catalogs.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}