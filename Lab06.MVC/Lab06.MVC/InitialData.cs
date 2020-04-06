using System.Threading.Tasks;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Identity;

namespace Lab06.MVC
{
    public class InitialData
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public InitialData(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            var adminEmail = "admin@admin.com";
            var password = "_admin123A";

            if (await _roleManager.FindByNameAsync("admin") == null)
                await _roleManager.CreateAsync(new IdentityRole("admin"));

            if (await _roleManager.FindByNameAsync("user") == null)
                await _roleManager.CreateAsync(new IdentityRole("user"));

            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User {Email = adminEmail, UserName = adminEmail};

                var result = await _userManager.CreateAsync(admin, password);

                if (result.Succeeded) await _userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}