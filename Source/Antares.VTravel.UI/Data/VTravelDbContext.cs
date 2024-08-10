namespace Antares.VTravel.UI.Data;
using Microsoft.EntityFrameworkCore;
using Antares.VTravel.UI.Core.Database;
using Antares.VTravel.Shared.Event;

public class VTravelDbContext(DbContextOptions<VTravelDbContext> options, DomainEventBus eventBus) : UnitOfWorkContext(options, eventBus)
{
    public const string SCHEMA = nameof(VTravel);

    public virtual DbSet<Tour> Tours { get; set; } = default!;

    override protected void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        OnModelCreatingLocal(builder);
    }

    private void OnModelCreatingLocal(ModelBuilder builder, bool referenceOnly = false)
    {
        builder.Entity<Tour>(b =>
        {
            b.ToTable(nameof(Tour), SCHEMA, t => t.ExcludeFromMigrations(referenceOnly));
            b.HasKey(s => s.Id);
            b.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId);
            b.Property(s => s.Name).HasMaxLength(200);
            b.Property(s => s.Active).HasDefaultValue(true);
        });
    }
}
