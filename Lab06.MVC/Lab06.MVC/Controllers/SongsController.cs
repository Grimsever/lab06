using System.Threading.Tasks;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class SongsController : Controller
    {
        private readonly IServices<Album> _albumServices;
        private readonly ILogger<SongsController> _logger;
        private readonly ISongServices _songServices;

        public SongsController(ISongServices songServices, IServices<Album> albumServices,
            ILogger<SongsController> logger)
        {
            _songServices = songServices;
            _albumServices = albumServices;
            _logger = logger;
        }

        // GET: Songs
        [AllowAnonymous]
        [Authorize(Roles = "user")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var musicCatalogContext = await _songServices.GetAll();

            return View(musicCatalogContext);
        }

        // GET: Songs/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["AlbumId"] = new SelectList(await _albumServices.GetAll(), "Id", "Name");

            return View();
        }

        // POST: Songs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Size,Genre,ArtistName,ReleaseDate,Name")]
            Song song)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation model creating song failed!!");

                ViewData["AlbumId"] = new SelectList(await _albumServices.GetAll(), "Id", "Name", song.Album.Id);

                return View(song);
            }

            await _songServices.Add(song);

            return RedirectToAction(nameof(Index));
        }

        // GET: Songs/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            ViewData["Album"] = new SelectList(await _albumServices.GetAll(), "Id", "Name");

            var viewSong = await _songServices.GetEditViewModel((int) id);

            return View(viewSong);
        }

        // POST: Songs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SongId, Name,Genre,ArtistName,ReleaseDate,AlbumId")]
            EditSongViewModel songModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation edit song is fail!!");

                ViewData["Album"] = new SelectList(await _albumServices.GetAll(), "Id", "Name");

                return View(songModel);
            }

            await _songServices.Update(songModel);

            return RedirectToAction(nameof(Index));
        }


        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var song = await _songServices.GetById((int) id);

            return View(song);
        }


        // POST: Songs/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _songServices.Remove(id);

            return RedirectToAction(nameof(Index));
        }
    }
}