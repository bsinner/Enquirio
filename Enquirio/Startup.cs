using Enquirio.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

        public virtual void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            // Add DB
            services.AddDbContext<DbContextEnq>(options => {
                options.UseSqlServer(
                    Configuration["ConnectionStrings:DefaultConnection"]
                );
            });

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();

            // Needed for UseSpaStaticFiles in Configure
            services.AddSpaStaticFiles(config => config.RootPath = "frontend/dist");
        }

        public virtual void Configure(IApplicationBuilder builder, IHostingEnvironment env) {

            builder.UseSpaStaticFiles();

            builder.UseSpa(spa => {
                spa.Options.SourcePath = "frontend";

                if (env.IsDevelopment()) {
                    // Run npm serve on port 8080
                    spa.UseVueCli("serve");
                }
            });

        }
    }
}
