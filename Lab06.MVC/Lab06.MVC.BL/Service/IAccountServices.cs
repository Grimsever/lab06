using System.Collections.Generic;
using Lab06.MVC.Domain.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Lab06.MVC.BL.Service
{
    public interface IAccountServices
    {
        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        IList<AuthenticationScheme> ExternalLogins { get; set; }
        void LogOut();
        void SignUp(RegisterModel model);
        void LogIn(LoginViewModel model);
    }
}