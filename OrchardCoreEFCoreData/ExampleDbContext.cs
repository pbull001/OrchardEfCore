using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OrchardCoreEFCoreData
{
    public class ExampleDbContext : DbContext
    {
        private readonly ILogger<ExampleDbContext> _logger;
        private string? _schema;

        public ExampleDbContext(ILogger<ExampleDbContext> logger,DbContextOptions<ExampleDbContext> options) : base(options)
        {
            _logger = logger;
            _schema = null;
            Console.WriteLine("OverwatchDbContext created without schema");
        }
        public ExampleDbContext(
            ILogger<ExampleDbContext> logger,
            DbContextOptions<ExampleDbContext> options, 
            string schema) : base(options)
        {
            _logger = logger;
            _schema = schema;
            _logger.LogInformation($"OverwatchDbContext created with schema: {_schema}");
        }

        // ------------------------------------------------------------------
        // User Domain

       
        public DbSet<Event>? Events { get; set; }

        
        
        //---------------------------------------------------------------------

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (!string.IsNullOrWhiteSpace(_schema))
            {
                modelBuilder.HasDefaultSchema(_schema);
                _logger.LogInformation($"OnModelCreating: Using schema {_schema}");
            }
            else
            {
                _logger.LogInformation("OverwatchDbContext created WITHOUT schema");
            }

                var jsonNodeComparer = new ValueComparer<JsonNode?>(
                    (l, r) => l == null && r == null || (l != null && r != null && l.ToJsonString((JsonSerializerOptions?)null) == r.ToJsonString((JsonSerializerOptions?)null)),
                    v => v == null ? 0 : v.ToJsonString((JsonSerializerOptions?)null).GetHashCode(),
                    v => v == null ? null : (JsonNode?)JsonNode.Parse(v.ToJsonString((JsonSerializerOptions?)null), null, default)
                    );


            // Define this at the start of your OnModelCreating
            var jsonConverter = new ValueConverter<JsonNode?, string?>(
                model => model == null ? null : model.ToJsonString((JsonSerializerOptions?)null),
                provider => provider == null ? null : (JsonNode?)JsonNode.Parse(provider, null, default)
                );

            //A. Events
            //==========

            modelBuilder.Entity<Event>(events =>
            {
                // Chain these methods with a dot (.)
                events.ToTable("Event");
                events.HasKey(e => e.EventId);
                events.Property(e => e.EventDateTime)
               .HasConversion(
                   v => v.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        : v.ToUniversalTime(),
                   v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                events.Property(e => e.IngestedDateTime)
               .HasConversion(
                   v => v == null ? (DateTime?)null
                        : v.Value.Kind == DateTimeKind.Unspecified
                             ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                             : v.Value.ToUniversalTime(),
                   v => v == null ? (DateTime?)null : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc));
                events.Property(o => o.Data)
                      .HasColumnName("Data")
                      .HasColumnType("nvarchar(max)")
                      .HasConversion(jsonConverter)
                      .Metadata.SetValueComparer(jsonNodeComparer);
                events.HasIndex(o => o.EventId);
            });

            

            base.OnModelCreating(modelBuilder);
        }


    }
}
