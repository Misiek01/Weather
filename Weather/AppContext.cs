using Microsoft.EntityFrameworkCore;

namespace Weather
{
    public partial class MainWindow
    {
        //Baza danych
        class AppContext : DbContext
        {
            public DbSet<Cities> Cities { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("DATASOURCE=data.db");
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder
                    .Entity<Cities>()
                    .ToTable("cities")
                    .HasData(
                        new Cities() { Id = 1, Name = "Warszawa", Place = 0 },
                        new Cities() { Id = 2, Name = "Madryt", Place = 1 },
                        new Cities() { Id = 3, Name = "Berlin", Place = 2 },
                        new Cities() { Id = 4, Name = "Poznań", Place = 3 }
                    );

            }
        }
        public record Cities
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Place { get; set; }
        }
        }
    }
