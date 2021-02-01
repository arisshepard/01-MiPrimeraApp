using _01_MiPrimeraApp.Server.Mappers;
using _01_MiPrimeraApp.Server.Models;
using _01_MiPrimeraApp.Server.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace _01_MiPrimeraApp.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSweetAlert2();
            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddDbContext<BDBibliotecaContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("BDBiblioteca")));

            ServiciosPaginasTipoUsuario(services);
            ServiciosTiposUsuario(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }

        private void ServiciosPaginasTipoUsuario(IServiceCollection services)
        {
            services.TryAddSingleton<IMapper<PaginaTipoUsuario, Shared.PaginaTipoUsuario>, PaginaTipoUsuarioMapper>();
            services.TryAddSingleton<IMapper<IEnumerable<PaginaTipoUsuario>, IEnumerable<Shared.PaginaTipoUsuario>>, EnumerableMapper<PaginaTipoUsuario, Shared.PaginaTipoUsuario>>();

            //services.TryAddSingleton<IPaginaTipoUsuarioMappingService, PaginaTipoUsuarioMappingService>();

            services.TryAddSingleton<IEntityMappingService<PaginaTipoUsuario, Shared.PaginaTipoUsuario>, EntityMappingService<PaginaTipoUsuario, Shared.PaginaTipoUsuario>>();
        }

        private void ServiciosTiposUsuario(IServiceCollection services)
        {
            services.TryAddSingleton<IMapper<TipoUsuario, Shared.TipoUsuario>, TipoUsuarioMapper>();
            services.TryAddSingleton<IMapper<IEnumerable<TipoUsuario>, IEnumerable<Shared.TipoUsuario>>, EnumerableMapper<TipoUsuario, Shared.TipoUsuario>>();

            //services.TryAddSingleton<ITipoUsuarioMappingService, TipoUsuarioMappingService>();
            services.TryAddSingleton<IEntityMappingService<TipoUsuario, Shared.TipoUsuario>, EntityMappingService<TipoUsuario, Shared.TipoUsuario>>();
        }
    }
}
