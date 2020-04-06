using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Lab06.MVC.Data.Context
{
    public class MusicCatalogContext : IdentityDbContext
    {
        public MusicCatalogContext(DbContextOptions<MusicCatalogContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(x => x.Catalogs)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);


            builder.Entity<CatalogSong>()
                .HasKey(x => new {x.CatalogId, x.SongId});

            builder.Entity<CatalogSong>()
                .HasOne(x => x.Catalog)
                .WithMany(x => x.CatalogSong)
                .HasForeignKey(x => x.CatalogId);

            builder.Entity<CatalogSong>()
                .HasOne(x => x.Song)
                .WithMany(x => x.CatalogSong)
                .HasForeignKey(x => x.SongId);

            base.OnModelCreating(builder);
        }
    }
}