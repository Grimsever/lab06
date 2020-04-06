using Lab06.MVC.BL.Service;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Lab06.MVC.Controllers
{
    [Authorize]
    public class AlbumsController : Controller
    {
        private readonly IServices<Album> _albumServices;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IServices<Album> albumServices, ILogger<AlbumsController> logger)
        {
            _albumServices = albumServices;
            _logger = logger;
        }

        // GET: Albums
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _albumServices.GetAll());
        }


        // GET: Albums/Create
        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Albums/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Album album)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation schema data album failed.");

                return View(album);
            }

            await _albumServices.Add(album);

            return RedirectToAction(nameof(Index));
        }

        // GET: Albums/Edit/5
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var album = await _albumServices.GetById((int)id);

            return View(album);
        }

        // POST: Albums/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Album album)
        {

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Validation album schema data failed.");

                return View(album);
            }

            await _albumServices.Update(id, album);

            return RedirectToAction(nameof(Index));
        }

        // GET: Albums/Delete/5
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var album = await _albumServices.GetById((int)id);

            return View(album);
        }

        // POST: Albums/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _albumServices.Remove(id);

            return RedirectToAction(nameof(Index));
        }
    }
}