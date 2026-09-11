using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrchardCore.Data;
using OrchardCore.Environment.Shell.Configuration;

namespace OrchardCoreEFCoreData
{
    public static class OrchardEFCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddOverwatchDatabase(this IServiceCollection services)
        {
            // 1. Register the "How" (Options)
            services.AddDbContext<ExampleDbContext>((sp, options) =>
            {
                var shellConfig = sp.GetRequiredService<IShellConfiguration>();
                var conn = shellConfig["ConnectionString"];
                var provider = shellConfig["DatabaseProvider"];

                if (provider == "SqlConnection")
                {
                    options.UseSqlServer($"{conn};MultipleActiveResultSets=True;");
                }
                else if (provider == "Sqlite")
                {
                    var accessor = sp.GetRequiredService<IDbConnectionAccessor>();
                    
                    options.UseSqlite(accessor.CreateConnection());
                    //options.UseSqlite(conn);
                }
                else
                {
                    throw new NotSupportedException($"Unsupported provider '{provider}'.");
                }
            });

            // 2. Register the "Who" (Instance Factory)
            services.AddScoped<ExampleDbContext>(sp =>
            {
                var options = sp.GetRequiredService<DbContextOptions<ExampleDbContext>>();
                var logger = sp.GetRequiredService<ILogger<ExampleDbContext>>();
                var shellConfig = sp.GetRequiredService<IShellConfiguration>();
                var dbProvider = (string)shellConfig["DatabaseProvider"] ?? "none";

                string? schema;
                switch (dbProvider.ToLower())
                {
                    case "sqlconnection":
                        schema = shellConfig["Schema"] ?? "dbo";
                        break;
                    case "sqlite":
                        schema = null;
                        break;
                    default:
                        schema = "dbo";
                        break;
                }
                //var schema = shellConfig["TablePrefix"] ?? shellConfig["Schema"] ?? "dbo";

                if (schema is null)
                {
                    return new ExampleDbContext(logger, options);
                }
                else
                {
                    return new ExampleDbContext(logger, options, schema);
                }
            });

            return services;
        }
    }
}