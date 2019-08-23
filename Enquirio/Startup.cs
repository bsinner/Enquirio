using Enquirio.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enquirio {
    public class Startup {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddRouting();
            services.AddRazorPages();
            
            services.AddDbContext<DbContextEnq>(options => {
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"]
                );
            });

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();
            services.AddSingleton(Environment);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });

        }
    }
}
