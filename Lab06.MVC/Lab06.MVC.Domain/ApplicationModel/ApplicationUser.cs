using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Identity;

namespace Lab06.MVC.Domain.ApplicationModel
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
