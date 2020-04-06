using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Data.Repository;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.BL.Implementation
{
    internal class AlbumServices : IServices<Album>
    {
        private readonly IRepository<Album, int> _albumRepository;
        private readonly ILogger<AlbumServices> _logger;

        public AlbumServices(ILogger<AlbumServices> logger, IRepository<Album, int> albumRepository)
        {
            _logger = logger;
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<Album>> GetAll()
        {
            var albums = await _albumRepository.GetAll();

            return albums;
        }

        public async Task Add(Album album)
        {
            CheckIsNullArg(album);

            await _albumRepository.Insert(album);
        }

        public async Task<Album> GetById(int id)
        {
            CheckId(id);

            var album = await _albumRepository.Get(id);

            CheckIsNullArg(album);

            return album;
        }

        public async Task Update(int id, Album album)
        {
            CheckId(id);

            CheckIsNullArg(album);

            if (id != album.Id)
            {
                _logger.LogError("Id and album.id must be equal.");

                throw new ArgumentException("Id and album.id must be equal.", nameof(id));
            }

            if (await IsExist(album.Id))
            {
                _logger.LogError("Album for update doesn`t exit in storage.");

                throw new ArgumentNullException(nameof(album), "Album for update doesn`t exit in storage.");
            }

            await _albumRepository.Update(album);
        }

        public async Task Remove(int id)
        {
            await _albumRepository.Remove(await GetById(id));
        }


        private void CheckId(int id)
        {
            if (id <= 0)
            {
                _logger.LogError("Id can`t be less than or equal to zero.");

                throw new ArgumentException($"Id value {id} can`t be less or equal to zero.");
            }
        }


        private async Task<bool> IsExist(int id)
        {
            return await GetById(id) == null;
        }

        private void CheckIsNullArg<T>(T item)
        {
            if (item == null)
            {
                _logger.LogError("Album can`t be null");

                throw new ArgumentNullException(nameof(item), "Album can`t be null.");
            }
        }
    }
}