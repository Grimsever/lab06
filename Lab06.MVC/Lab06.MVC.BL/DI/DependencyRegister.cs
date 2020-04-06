using Lab06.MVC.BL.Implementation;
using Lab06.MVC.BL.Service;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.Extensions.DependencyInjection;

namespace Lab06.MVC.BL.DI
{
    public static class DependencyRegister
    {
        public static void RegisterBusinessComponents(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISongServices, SongServices>();

            serviceCollection.AddScoped<IServices<Album>, AlbumServices>();

            serviceCollection.AddScoped<ICatalogServices, CatalogServices>();

        }
    }
}