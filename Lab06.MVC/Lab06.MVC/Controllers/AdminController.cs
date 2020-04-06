using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly MusicCatalogContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly UserManager<User> _userManager;

        public AdminController(UserManager<User> userManager, MusicCatalogContext context,
            RoleManager<IdentityRole> roleManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _logger = logger;
        }


        // GET: Admin
        [HttpGet]
        public IActionResult Index()
        {
            var model = new List<UserViewModel>();

            foreach (var user in _userManager.Users)
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    RoleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
                });

            return View(model);
        }

        // GET: Admin/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
                return RedirectToAction(nameof(Index));

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                _logger.LogWarning($"User with id: {id} not found!");

                return RedirectToAction(nameof(Index));
            }

            var model = new EditUserViewModel
            {
                ApplicationRoles = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Id
                }).ToList()
            };


            model.Name = user.UserName;

            model.ApplicationRoleId = _roleManager.Roles
                .Single(r => r.Name == _userManager
                    .GetRolesAsync(user).Result
                    .Single()).Id;

            return View(model);
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return View(model);

            user.UserName = model.Name;

            var resultUpdateUser = await _userManager.UpdateAsync(user);

            if (!resultUpdateUser.Succeeded)
            {
                ModelState.AddModelError("", resultUpdateUser.Errors.First().ToString());

                return View(model);
            }

            var currentRole = _userManager.GetRolesAsync(user).Result.Single();

            var currentRoleId = _roleManager.Roles.Single(r => r.Name == currentRole).Id;

            if (currentRoleId == model.ApplicationRoleId)
                return RedirectToAction(nameof(Index));

            var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole);

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", removeResult.Errors.First().ToString());

                return View(model);
            }

            var newRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);

            if (newRole == null)
            {
                _logger.LogWarning("New role not found.");

                return View(model);
            }

            var newRoleResult = await _userManager.AddToRoleAsync(user, newRole.Name);

            if (!newRoleResult.Succeeded)
            {
                ModelState.AddModelError("", newRoleResult.Errors.First().ToString());

                return View(model);
            }

            return RedirectToAction("Index");
        }

        // GET: Admin/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                _logger.LogWarning($"User with id: {id} not found.");

                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.Users
                .Include(x => x.Catalogs)
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}