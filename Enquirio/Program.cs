using System.Threading.Tasks;
using Enquirio.Data;
using Enquirio.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Enquirio {
    public class Program {
        public static async Task Main(string[] args) {
            var host = CreateWebHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();

            var provider = scope.ServiceProvider;
            var context = provider.GetRequiredService<DbContextEnq>();
            var config = provider.GetRequiredService<IConfiguration>();
            
            await DbInitializer.Initialize(context, provider, config);
            host.Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(w => w.UseStartup<Startup>());
    }
}
