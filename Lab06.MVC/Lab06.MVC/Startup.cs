using Lab06.MVC.BL.DI;
using Lab06.MVC.BL.Filter;
using Lab06.MVC.Data;
using Lab06.MVC.Data.Context;
using Lab06.MVC.Domain.RepositoryModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lab06.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public async void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MusicCatalogContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            services.AddDefaultIdentity<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MusicCatalogContext>();

            services.AddScoped<InitialData>();

            services.RegistrationDataComponent();

            services.RegisterBusinessComponents();

            services.AddMvc().AddRazorRuntimeCompilation();

            services.AddControllersWithViews(opt =>
                opt.Filters.Add(typeof(GlobalExceptionFilter))
            );

            await services.BuildServiceProvider().GetService<InitialData>().InitializeAsync();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}