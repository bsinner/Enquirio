using Enquirio.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DbContextEnq>()
                .AddDefaultTokenProviders();

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();
            services.AddSingleton(Environment);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<IdentityOptions>(i => {
                i.User.AllowedUserNameCharacters
                    = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
            });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}
