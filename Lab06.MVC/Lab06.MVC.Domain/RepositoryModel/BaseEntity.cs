using System.ComponentModel.DataAnnotations;

namespace Lab06.MVC.Domain.RepositoryModel
{
    public class BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(20)] [Required] public string Name { get; set; }
    }
}