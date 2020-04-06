using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.OwException;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.BL.Implementation
{
    internal class AdminServices : IAdminServices
    {
        private readonly MusicCatalogContext _context;
        private readonly ILogger<AdminServices> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public AdminServices(UserManager<User> userManager, MusicCatalogContext context,
            RoleManager<IdentityRole> roleManager, ILogger<AdminServices> logger)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            var model = new List<UserViewModel>();

            foreach (var user in await _userManager.Users.ToListAsync())
                model.Add(new UserViewModel
                {
                    Id = user.Id,
                    Name = user.UserName,
                    RoleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
                });

            return model;
        }

        public async Task<EditUserViewModel> GetEditViewUser(string id)
        {
            var user = await Get(id);

            var model = new EditUserViewModel
            {
                ApplicationRoles =
                    _roleManager.Roles
                        .Select(r => new SelectListItem
                        {
                            Text = r.Name,
                            Value = r.Id
                        })
                        .ToList(),

                Name = user.UserName,

                ApplicationRoleId = _roleManager.Roles
                    .Single(r => r.Name == _userManager
                        .GetRolesAsync(user).Result
                        .Single()).Id
            };

            return model;
        }

        public async Task<User> Get(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }


        public async void Update(string id, EditUserViewModel model)
        {
            var user = await Get(id);

            if (user == null)
                throw new UserNotFoundException(nameof(user));

            user.UserName = model.Name;

            var resultUpdateUser = await _userManager.UpdateAsync(user);

            if (!resultUpdateUser.Succeeded)
            {
                _logger.LogError(resultUpdateUser.Errors.First().ToString());

                throw new DbUpdateException(resultUpdateUser.Errors.First().ToString());
            }

            var currentRole = _userManager.GetRolesAsync(user).Result.Single();

            var currentRoleId = _roleManager.Roles.Single(r => r.Name == currentRole).Id;

            if (currentRoleId == model.ApplicationRoleId)
                return;

            var removeResult = await _userManager.RemoveFromRoleAsync(user, currentRole);

            if (!removeResult.Succeeded)
            {
                _logger.LogError(removeResult.Errors.First().ToString());

                throw new DbUpdateException(removeResult.Errors.First().ToString());
            }


            var newRole = await _roleManager.FindByIdAsync(model.ApplicationRoleId);

            if (newRole == null)
            {
                _logger.LogWarning("New role not found.");

                throw new DbUpdateException("New role not found.");
            }

            var newRoleResult = await _userManager.AddToRoleAsync(user, newRole.Name);

            if (!newRoleResult.Succeeded)
            {
                _logger.LogWarning(newRoleResult.Errors.First().ToString());

                throw new DbUpdateException(newRoleResult.Errors.First().ToString());
            }
        }

        public async void Remove(string id)
        {
            var user = await _userManager.Users
                .Include(x => x.Catalogs)
                .FirstOrDefaultAsync(x => x.Id == id);

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }
    }
}