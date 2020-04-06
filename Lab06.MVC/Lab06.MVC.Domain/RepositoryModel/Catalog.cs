using System.Collections.Generic;

namespace Lab06.MVC.Domain.RepositoryModel
{
    public class Catalog : BaseEntity
    {
        public Catalog()
        {
            CatalogSong = new List<CatalogSong>();
        }

        public User User { get; set; }

        public ICollection<CatalogSong> CatalogSong { get; set; }
    }
}