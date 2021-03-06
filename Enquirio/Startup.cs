﻿using System;
using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Infrastructure;
using Enquirio.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DbContextEnq>()
                .AddDefaultTokenProviders();

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();
            services.AddSingleton(Environment);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<IdentityOptions>(i => {
                i.User.AllowedUserNameCharacters
                    = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._+";
            });

            services.AddAuthorization(opts => {
                opts.AddPolicy("AuthorOnly", p => {
                    p.AddRequirements(new AuthorOnlyRequirement(new [] { "Admin" }));
                });
            });

            // Override MVC redirecting to login page on 401 or 403 and returning 404
            services.ConfigureApplicationCookie(opts => {
                opts.Events.OnRedirectToAccessDenied = cxt => SetCode(cxt, 403);
                opts.Events.OnRedirectToLogin = cxt => SetCode(cxt, 401);
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

        private readonly Func<RedirectContext<CookieAuthenticationOptions>, int, Task> 
                SetCode = (cxt, code) => {

            cxt.Response.Headers["Location"] = cxt.RedirectUri;
            cxt.Response.StatusCode = code;
            return Task.CompletedTask;
        };
    }
}
