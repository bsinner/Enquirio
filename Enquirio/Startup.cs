using Enquirio.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VueCliMiddleware;

namespace Enquirio {
    public class Startup {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // Set up services
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            // Add DB
            services.AddDbContext<DbContextEnq>(options => {
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"]
                );
            });

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();

        }

        // Middleware
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();

                #if DEBUG
                    if (System.Diagnostics.Debugger.IsAttached) {
                        endpoints.MapToVueCliProxy("{*path}"
                            , new SpaOptions {SourcePath = "frontend"}
                            , "serve", regex: "Compiled successfully");
                    }
                #endif

                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
