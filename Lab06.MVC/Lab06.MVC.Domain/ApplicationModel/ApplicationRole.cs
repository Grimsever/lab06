using Microsoft.AspNetCore.Identity;

namespace Lab06.MVC.Domain.ApplicationModel
{
    public class ApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
