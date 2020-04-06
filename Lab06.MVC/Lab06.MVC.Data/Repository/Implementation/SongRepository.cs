using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.EntityFrameworkCore;

namespace Lab06.MVC.Data.Repository.Implementation
{
    internal class SongRepository : IRepository<Song, int>
    {
        private readonly MusicCatalogContext _context;

        public SongRepository(MusicCatalogContext context)
        {
            _context = context;
        }

        public async Task<Song> Get(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id can`t be less than or equal to zero.");

            var song = await _context.Songs
                .Include(x => x.Album)
                .Include(x => x.CatalogSong)
                .ThenInclude(x=>x.Catalog)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return song;
        }

        public async Task<IEnumerable<Song>> GetAll()
        {
            var songs = await _context.Songs
                .Include(x => x.Album)
                .Include(x => x.CatalogSong)
                .ThenInclude(x=>x.Catalog)
                .AsNoTracking()
                .ToListAsync();

            return songs;
        }

        public async Task Insert(Song item)
        {
            _context.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task Remove(Song item)
        {
            _context.Songs.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task Update(Song item)
        {
            _context.Songs.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}