﻿using Enquirio.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Enquirio {
    public class Startup {

        public IConfiguration Configuration { get; }
        private string URL =
            "Server=(localdb)\\MSSQLLocalDB;Database=Enquirio;"
            + "Trusted_Connection=True;MultipleActiveResultSets=true";

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        // Set up services
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services.AddDbContext<DbContextEnq>
                    (options => options.UseSqlServer(URL));

            services.AddScoped<IRepositoryEnq, RepositoryEnq>();
        }

        // Middleware
        public void Configure(IApplicationBuilder builder, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                builder.UseDeveloperExceptionPage();
            }

            builder.UseStaticFiles();

            builder.UseMvcWithDefaultRoute();

        }
    }
}
