using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DAMS.Models;

namespace DAMS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DAMS.Models.User> User { get; set; } = default!;
        public DbSet<DAMS.Models.Doctor> Doctor { get; set; } = default!;
        public DbSet<DAMS.Models.Driver> Driver { get; set; } = default!;
        public DbSet<DAMS.Models.Appointment> Appointment { get; set; } = default!;
    }
}
