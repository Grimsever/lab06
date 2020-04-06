using System.Collections.Generic;
using System.Text;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Domain.RepositoryModel;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace Lab06.MVC.BL.Implementation
{
    internal class AccountServices : IAccountServices
    {
        private readonly ILogger<AccountServices> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AccountServices(SignInManager<User> signInManager, UserManager<User> userManager,
            ILogger<AccountServices> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public string ReturnUrl { get; set; }
        public string ErrorMessage { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public async void LogOut()
        {
            await _signInManager.SignOutAsync();
        }

        public async void SignUp(RegisterModel model)
        {
            var user = new User {UserName = model.Email, Email = model.Email};

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                await _userManager.AddToRoleAsync(user, "user");

                await _signInManager.SignInAsync(user, false);
            }
        }

        public async void LogIn(LoginViewModel model)
        {
            await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        }
    }
}