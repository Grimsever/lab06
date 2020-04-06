using Lab06.MVC.BL.Service;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Lab06.MVC.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Lab06.MVC.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class CatalogsController : Controller
    {
        private readonly ICatalogServices _catalogServices;
        private readonly IServices<Album> _albumServices;
        private readonly ISongServices _songServices;
        private readonly ILogger<CatalogsController> _logger;
        private readonly UserManager<User> _userManager;

        public CatalogsController(ICatalogServices catalogServices, IServices<Album> albumServices,
            ISongServices songServices, ILogger<CatalogsController> logger, UserManager<User> userManager)
        {
            _catalogServices = catalogServices;
            _albumServices = albumServices;
            _songServices = songServices;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: Catalogs
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var catalogs = await _catalogServices.GetAll();

            ViewBag.Albums = await _albumServices.GetAll();
            ViewBag.Number = 0;
            ViewBag.Songs = await _songServices.GetAll();

            return View(catalogs.ToList());
        }

        // Get: Catalogs/AddAlbum/{catalogId=3, albumId=3}
        [HttpGet]
        public async Task<IActionResult> AddAlbum(int catalogId, int albumId)
        {
            await _catalogServices.AddAlbumToCatalog(catalogId, albumId);

            return RedirectToAction(nameof(Index));
        }


        //Get: Catalogs/DeleteSong/{catalogId = 3, songId = 3}
        [HttpGet]
        public async Task<IActionResult> DeleteSong(int catalogId, int songId)
        {
            await _catalogServices.RemoveSongFromCatalog(catalogId, songId);

            return RedirectToAction(nameof(Index));
        }

        // Get: Catalogs/AddSong/{songId = 3, catalogId = 3}
        [HttpGet]
        public async Task<IActionResult> AddSong(int songId, int catalogId)
        {
            await _catalogServices.AddSongToCatalog(catalogId, songId);

            return RedirectToAction(nameof(Index));
        }

        // GET: Catalogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catalogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Catalog catalog)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation schema data new catalog failed.");

                return View(catalog);
            }

            await _catalogServices.Add(catalog);

            return RedirectToAction(nameof(Index));
        }

        // GET: Catalogs/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var catalog = await _catalogServices.GetById((int)id);

            return View(catalog);
        }

        // POST: Catalogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Id,Name")] Catalog catalog)
        {
            if (id != catalog.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {

                _logger.LogWarning("Validation edit catalog failed.");

                return View(catalog);
            }

            await _catalogServices.Update(id, catalog);

            return RedirectToAction(nameof(Index));
        }

        // GET: Catalogs/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var catalog = await _catalogServices.GetById((int)id);

            return View(catalog);
        }

        // POST: Catalogs/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _catalogServices.Remove(id);

            return RedirectToAction(nameof(Index));
        }
    }
}