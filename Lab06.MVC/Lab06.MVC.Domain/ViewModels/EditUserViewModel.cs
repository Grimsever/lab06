using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab06.MVC.Domain.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Role")] public string ApplicationRoleId { get; set; }
    }
}