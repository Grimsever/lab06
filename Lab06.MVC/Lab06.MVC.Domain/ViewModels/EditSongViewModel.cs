using System;
using System.ComponentModel.DataAnnotations;

namespace Lab06.MVC.Domain.ViewModels
{
    public class EditSongViewModel
    {
        public int SongId { get; set; }
        public string Name { get; set; }
        public string Genre { get; set; }
        public string ArtistName { get; set; }

        [DataType(DataType.Date)] public DateTime ReleaseDate { get; set; }

        public int? AlbumId { get; set; }
    }
}