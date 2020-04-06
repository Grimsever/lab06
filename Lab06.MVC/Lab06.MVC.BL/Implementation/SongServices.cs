using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Data.Repository;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.BL.Implementation
{
    internal class SongServices : ISongServices
    {
        private readonly IRepository<Album, int> _albumRepository;
        private readonly ILogger<SongServices> _logger;
        private readonly IRepository<Song, int> _songRepository;

        public SongServices(IRepository<Song, int> songRepository, ILogger<SongServices> logger,
            IRepository<Album, int> albumRepository)
        {
            _songRepository = songRepository;
            _logger = logger;
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<Song>> GetAll()
        {
            return await _songRepository.GetAll();
        }

        public async Task Add(Song item)
        {
            CheckIsNullArg(item);

            await _songRepository.Insert(item);
        }

        public async Task<Song> GetById(int id)
        {
            CheckId(id);

            var song = await _songRepository.Get(id);

            CheckIsNullArg(song);

            return song;
        }

        public async Task Update(int id, Song item)
        {
            CheckId(id);

            CheckIsNullArg(item);

            if (id != item.Id)
            {
                _logger.LogError($"Id: {id} and song.id: {item.Id} for update must be equal.");

                throw new ArgumentException($"Id: {id} and song.id: {item.Id} for update must be equal.");
            }

            if (await IsExist(id))
            {
                _logger.LogError($"Song with id:{id} not found in storage");

                throw new ArgumentException($"Song with id:{id} not found in storage");
            }

            await _songRepository.Update(item);
        }

        public async Task Remove(int id)
        {
            CheckId(id);

            if (await IsExist(id))
                throw new ArgumentNullException(nameof(id), "Song doesn`t exist in storage.");

            await _songRepository.Remove(await GetById(id));
        }

        public async Task<EditSongViewModel> GetEditViewModel(int id)
        {
            CheckId(id);

            var song = await _songRepository.Get(id);

            CheckIsNullArg(song);

            var viewSong = new EditSongViewModel
            {
                SongId = song.Id,
                Name = song.Name,
                ArtistName = song.ArtistName,
                Genre = song.Genre,
                ReleaseDate = song.ReleaseDate,
                AlbumId = song.Album?.Id
            };

            return viewSong;
        }

        public async Task Update(EditSongViewModel songModel)
        {
            var song = await GetById(songModel.SongId);

            song.Name = songModel.Name;
            song.Genre = songModel.Genre;
            song.ArtistName = songModel.ArtistName;
            song.ReleaseDate = songModel.ReleaseDate;

            if (songModel.AlbumId != null) song.Album = await _albumRepository.Get((int) songModel.AlbumId);

            await _songRepository.Update(song);
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
                _logger.LogError("Song can`t be null");

                throw new ArgumentNullException(nameof(item), "Song can`t be null.");
            }
        }
    }
}