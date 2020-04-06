using Lab06.MVC.Data.Repository;
using Lab06.MVC.Data.Repository.Implementation;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Lab06.MVC.Data
{
    public static class DependencyRegistration
    {
        public static void RegistrationDataComponent(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IRepository<Catalog, int>, CatalogRepository>();

            serviceCollection.AddScoped<IRepository<Album, int>, AlbumRepository>();

            serviceCollection.AddScoped<IRepository<Song, int>, SongRepository>();

            serviceCollection.AddScoped<IRepository<IdentityUser, string>, UserRepository>();

            serviceCollection.AddScoped<ICatalogSongRepository, CatalogSongRepository>();
        }
    }
}