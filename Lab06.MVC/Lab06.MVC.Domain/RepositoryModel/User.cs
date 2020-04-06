using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Lab06.MVC.Domain.RepositoryModel
{
    public class User : IdentityUser
    {
        public User()
        {
            Catalogs = new List<Catalog>();
        }

        [Required] public ICollection<Catalog> Catalogs { get; set; }
    }
}