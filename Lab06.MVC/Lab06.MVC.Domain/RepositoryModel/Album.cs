using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab06.MVC.Domain.RepositoryModel
{
    [Table("Album")]
    public class Album : BaseEntity
    {
        public Album()
        {
            Songs = new List<Song>();
        }

        public ICollection<Song> Songs { get; set; }
    }
}