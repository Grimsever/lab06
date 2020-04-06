namespace Lab06.MVC.Domain.RepositoryModel
{
    public class CatalogSong
    {
        public int SongId { get; set; }
        public Song Song { get; set; }
        public int CatalogId { get; set; }
        public Catalog Catalog { get; set; }
    }
}