using Enquirio.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enquirio {
    public class Startup {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // Set up services
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddRazorPages();

            services.AddDbContext<DbContextEnq>(options => {
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"]
                );
            });

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();
        }

        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
                    endpoints.MapControllerRoute("question", "question/{id?}",
                        new {controller = "Question", action = "ViewQuestion"});
            });
        }
    }
}
