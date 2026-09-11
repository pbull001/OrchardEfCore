

using OrchardCoreEFCoreData;
using OrchardCore.Data.Migration;
using OrchardCore.Environment.Shell.Configuration;
using YesSql;
using YesSql.Sql;

namespace OrchardCoreEFCoreData
{
    public class Migrations : DataMigration
    {
        private readonly ISession _session;
        private readonly IShellConfiguration _shellConfiguration;
        private readonly ISqlDialect _dialect; // Added
        private readonly ExampleDbContext _dbContext;

        // Constructor added to fetch the Dialect from YesSql
        public Migrations(IShellConfiguration shellConfiguration, ISession session, ExampleDbContext dbContext)
        {
            _session = session;
            _shellConfiguration = shellConfiguration;
            _dialect = session.Store.Configuration.SqlDialect;
            _dbContext = dbContext;
        }

        public async Task<int> Create()
        {
            // Get the table prefix from the configuration
            var prefix = _shellConfiguration["TablePrefix"] ?? string.Empty;
            var schema = _shellConfiguration["Schema"] ?? string.Empty;

            

            //-------------------------------------------------------------------------------------

            //Build Tables

           

            await SchemaBuilder.CreateTableAsync(nameof(Event), table => table
                .Column<long>(nameof(Event.EventId), column => column.PrimaryKey().Identity())
                .Column<DateTime>(nameof(Event.EventDateTime), column => column.NotNull())
                .Column<DateTime>(nameof(Event.IngestedDateTime), column => column.Nullable())
                .Column<string>(nameof(Event.DeviceReference), column => column.Nullable())
                .Column<string>(nameof(Event.DeviceType), column => column.Nullable())
                .Column<string>(nameof(Event.EventTypeCode), column => column.Nullable())
                .Column<int>(nameof(Event.LoopCount), column => column.Nullable())
                .Column<string>(nameof(Event.Source), column => column.Nullable())
                .Column<string>(nameof(Event.Destination), column => column.Nullable())
                .Column<string>(nameof(Event.Data), column => column.Nullable().Unlimited()) //Data?
                );

            return 1;
        }

        

    }
}
