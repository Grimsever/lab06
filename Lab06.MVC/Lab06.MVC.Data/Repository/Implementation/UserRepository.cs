using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.OwException;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Lab06.MVC.Data.Repository.Implementation
{
    internal class UserRepository : IRepository<IdentityUser, string>
    {
        private readonly MusicCatalogContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public UserRepository(MusicCatalogContext context, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityUser> Get(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "User id can`t be null.");

            return await _userManager.Users
                .Include(x => x.Catalogs)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            return await _userManager.Users
                .Include(x => x.Catalogs).ToListAsync();
        }

        public async Task Insert(IdentityUser item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "User item for insert can`t be null.");

            _context.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task Remove(IdentityUser user)
        {
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
        }


        public async Task Update(IdentityUser item)
        {
            if (item == null)
                throw new UserNotFoundException(nameof(item));

            _context.Users.Update(item);

            await _context.SaveChangesAsync();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}