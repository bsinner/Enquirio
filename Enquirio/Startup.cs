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
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();
            services.AddRouting();

            if (Environment.IsDevelopment()) {
                services.AddCors();
            }

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

            if (env.IsDevelopment()) {
                app.UseCors(c => {
                    c.WithOrigins(Configuration["VueCliUrl"])
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
