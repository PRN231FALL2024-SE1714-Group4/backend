using BOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BOs
{
    public class PiggeryManagementContext : DbContext
    {
        public PiggeryManagementContext(DbContextOptions<PiggeryManagementContext> options) : base(options)
        {
        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Cage> Cages { get; set; }
        public DbSet<History> Histories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Work> Works { get; set; }  // Fully qualify here
        public DbSet<UserShift> UserShifts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<History>()
                .HasKey(h => new { h.AnimalID, h.CageID });

            modelBuilder.Entity<Animal>()
                .HasMany(a => a.Histories)
                .WithOne(h => h.Animal)
                .HasForeignKey(h => h.AnimalID);

            modelBuilder.Entity<Cage>()
                .HasMany(c => c.Histories)
                .WithOne(h => h.Cage)
                .HasForeignKey(h => h.CageID);

            modelBuilder.Entity<Area>()
                .HasMany(a => a.Cages)
                .WithOne(c => c.Area)
                .HasForeignKey(c => c.AreaID);

            //modelBuilder.Entity<Task>().HasNoKey();
            modelBuilder.Entity<Work>()
                .HasKey(w => w.WorkId);
            
            modelBuilder.Entity<Work>()
                .HasOne(w => w.Assigner)
                .WithMany(u => u.AssignedByMe)
                .HasForeignKey(w => w.AssignerID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Work>()
                .HasOne(w => w.Assignee)
                .WithMany(u => u.AssignedWorks)
                .HasForeignKey(w => w.AssigneeID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .Property(r => r.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Work)
                .WithMany(w => w.Reports)
                .HasForeignKey(r => r.WorkId)
                .OnDelete(DeleteBehavior.NoAction); // Specify NoAction to avoid cycles

            modelBuilder.Entity<UserShift>()
                .HasOne(r => r.User);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(GetConnectionString());

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true).Build();
            return configuration["ConnectionStrings:DefaultConnectionString"];
        }
    }
}
