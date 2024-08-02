namespace Antares.VTravel.UI.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    /*

    dotnet ef migrations add InitialTour --startup-project ..\Antares.VTravel.UI\ -c ApplicationDbContext
    */

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<Tour> Tours { get; set; } = default!;

        override protected void OnModelCreating(ModelBuilder builder)
        {
            OnModelCreatingLocal(builder);
            base.OnModelCreating(builder);
        }

        private void OnModelCreatingLocal(ModelBuilder builder)
        {
            builder.Entity<Tour>(b =>
            {
                b.ToTable(nameof(Tour));

                b.HasKey(s => s.Id);

                b.HasOne(s => s.User).WithMany().HasForeignKey(t => t.UserId);

                b.Property(s => s.Name).HasMaxLength(200);
                b.Property(s => s.Active).HasDefaultValue(true);
            });
        }
    }
}
