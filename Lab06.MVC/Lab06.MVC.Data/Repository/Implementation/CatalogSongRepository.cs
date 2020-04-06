using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.OwException;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lab06.MVC.Data.Repository.Implementation
{
    internal class CatalogSongRepository : ICatalogSongRepository
    {
        private readonly MusicCatalogContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _accessor;

        public CatalogSongRepository(MusicCatalogContext context,
            UserManager<User> userManager, IHttpContextAccessor accessor)
        {
            _context = context;
            _userManager = userManager;
            _accessor = accessor;
        }


        public async Task Remove(int catalogId, int songId)
        {
            var song = _context.Songs.FirstOrDefault(x => x.Id == songId);

            if (song == null)
                throw new ArgumentException($"Song with id {songId} not found in storage.");

            var user = _userManager.GetUserAsync(_accessor.HttpContext.User).Result;

            if (user == null)
                throw new UserNotFoundException($"Already login user {nameof(user)} not found, please make relogin");

            var catalog = _context.Catalogs
                .Include(x => x.User)
                .Include(x => x.CatalogSong)
                .FirstOrDefault(x => x.User.Id == user.Id && x.Id == catalogId);

            if (catalog == null)
                throw new ArgumentException($"Catalog with id {catalogId} not found in storage.");

            var removeData = catalog.CatalogSong.FirstOrDefault(x => x.Song.Id == song.Id && x.Catalog.Id == catalog.Id);

            if (removeData == null)
                throw new ArgumentException($"Catalog {catalog.Name} doesn`t have song with id: {songId}");

            catalog.CatalogSong.Remove(removeData);

            await _context.SaveChangesAsync();
        }

        public Task Insert(CatalogSong item)
        {
            throw new NotImplementedException();
        }

        public Task Update(CatalogSong item)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
