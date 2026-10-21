using Microsoft.EntityFrameworkCore;

namespace Wpm.Clinic.DataAccess;

public class ClinicDbContext : DbContext
{
    public DbSet<Consultation> Consultations { get; set; }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
    {
    }

}
public record Consultation(Guid Id, int PatientId, string PatientName, int PatientAge, DateTime StartTime);

    public static class ClinicDbContextExtensions
    {
        public static void EnsureClinicDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ClinicDbContext>();
            context!.Database.EnsureCreated();
        }
    }
