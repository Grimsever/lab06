using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Data.Repository;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.BL.Implementation
{
    internal class CatalogServices : ICatalogServices
    {
        private readonly IRepository<Album, int> _albumRepository;
        private readonly IRepository<Catalog, int> _catalogRepository;
        private readonly ILogger<CatalogServices> _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;
        private readonly ICatalogSongRepository _catalogSongRepository;
        private readonly IRepository<Song, int> _songRepository;

        public CatalogServices(IRepository<Catalog, int> catalogRepository, IRepository<Album, int> albumRepository,
            IRepository<Song, int> songRepository, ILogger<CatalogServices> logger, IHttpContextAccessor accessor,
            UserManager<User> userManager, ICatalogSongRepository catalogSongRepository)
        {
            _catalogRepository = catalogRepository;
            _albumRepository = albumRepository;
            _songRepository = songRepository;
            _logger = logger;
            _accessor = accessor;
            _userManager = userManager;
            _catalogSongRepository = catalogSongRepository;
        }

        public async Task<IEnumerable<Catalog>> GetAll()
        {
            var user = await _userManager.GetUserAsync(_accessor.HttpContext.User);

            var catalogs = await _catalogRepository.GetAll();

            catalogs = catalogs.Where(x => x.User?.Id == user.Id);

            return catalogs;
        }


        public async Task Add(Catalog item)
        {
            CheckIsNullArg(item);

            var user = await _userManager.GetUserAsync(_accessor.HttpContext.User);

            item.User = user;

            await _catalogRepository.Insert(item);
        }

        public async Task<Catalog> GetById(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Id can`t be less than or equal to zero.");

                throw new ArgumentException("Id can`t be less than or equal to zero.", nameof(id));
            }

            var catalog = await _catalogRepository.Get(id);

            CheckIsNullArg(catalog);

            return catalog;
        }

        public async Task Update(int id, Catalog item)
        {
            CheckIsNullArg(item);

            if (await IsExist(item.Id))
                throw new ArgumentNullException(nameof(item), "Catalog for update doesn`t exit in storage.");

            await _catalogRepository.Update(item);
        }

        public async Task Remove(int id)
        {
            var catalog = await GetById(id);

            CheckIsNullArg(catalog);

            await _catalogRepository.Remove(catalog);
        }

        public async Task AddAlbumToCatalog(int catalogId, int albumId)
        {
            if (catalogId <= 0 || albumId <= 0)
            {
                _logger.LogError("Id can`t be less than or equal to zero.");

                throw new ArgumentException("Id can`t be less than or equal to zero.",
                    $"{nameof(albumId)} {nameof(catalogId)}");
            }

            var album = await _albumRepository.Get(albumId);

            if (album == null)
            {
                _logger.LogWarning($"Album with id: {albumId} not found.");

                throw new ArgumentNullException(nameof(album), $"Album with id {albumId} doesn`t exist in storage.");
            }

            var catalog = await _catalogRepository.Get(catalogId);

            if (catalog == null)
            {
                _logger.LogWarning($"Catalog with id: {catalogId} not found.");

                throw new ArgumentNullException(nameof(catalog),
                    $"Catalog with id {albumId} doesn`t exist in storage.");
            }

            foreach (var song in album.Songs)
            {
                var isExist = catalog.CatalogSong.Any(x => x.Catalog.Id == catalogId && x.Song.Id == song.Id);

                if (isExist)
                {
                    _logger.LogDebug(
                        $"Song: {song.Id} {song.Name} from album {album.Name} already exist in this catalog.");

                    continue;
                }

                catalog.CatalogSong.Add(new CatalogSong
                {
                    Catalog = catalog,
                    Song = song
                });
            }

            await _catalogRepository.Update(catalog);
        }

        public async Task RemoveSongFromCatalog(int catalogId, int songId)
        {
            await _catalogSongRepository.Remove(catalogId, songId);
        }

        public async Task AddSongToCatalog(int catalogId, int songId)
        {
            var song = await _songRepository.Get(songId);

            if (song == null)
            {
                _logger.LogWarning($"Song with id: {songId} not found");

                throw new ArgumentNullException(nameof(song), $"Song with id  {songId} not found in storage.");
            }


            var catalog = await _catalogRepository.Get(catalogId);

            if (catalog == null)
            {
                _logger.LogWarning($"Catalog by id {catalogId} not found!");

                throw new ArgumentNullException(nameof(catalog), $"Catalog with id: {catalogId} not found in storage.");
            }

            var isExist = catalog.CatalogSong.Any(x => x.Catalog?.Id == catalogId && x.Song?.Id == songId);

            if (isExist)
            {
                _logger.LogDebug($"That song with id: {songId} already exist in this catalog.");

                throw new ArgumentException($"Catalog {catalog.Name} with song {song.Name} already exist.",
                    nameof(catalog));
            }

            catalog.CatalogSong.Add(new CatalogSong
            {
                Catalog = catalog,
                Song = song
            });

            await _catalogRepository.Update(catalog);
        }

        private void CheckIsNullArg<T>(T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Catalog for update/add can`t be null.");
        }


        private async Task<bool> IsExist(int id)
        {
            return await _catalogRepository.Get(id) == null;
        }

    }
}