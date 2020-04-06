using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab06.MVC.Domain.RepositoryModel
{
    [Table("Song")]
    public class Song : BaseEntity
    {
        [MaxLength(20)]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "N/A")]
        public string Genre { get; set; }

        [MaxLength(30)]
        [DisplayFormat(DataFormatString = "Singer")]
        public string ArtistName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:yyyy-MM-dd}")]
        public DateTime ReleaseDate { get; set; }

        public Album Album { get; set; }

        public ICollection<CatalogSong> CatalogSong { get; set; }

        public Song()
        {
            CatalogSong = new List<CatalogSong>();
        }
    }
}