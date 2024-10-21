using Microsoft.EntityFrameworkCore;
namespace StarshipProject.Models
{
    //context to help setup the DB scaffolding
    public class StarshipContext : DbContext
    {
        public StarshipContext(DbContextOptions<StarshipContext> options) : base(options) {}

        public DbSet<Starship> Starships { get; set; }
        public DbSet<Pilot> Pilots { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<StarshipPilot> StarshipPilots { get; set; }
        public DbSet<StarshipFilm> StarshipFilms { get; set; }

        //sets up the partial keys between starships, pilots, and films
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StarshipPilot>()
                .HasKey(sp => new { sp.StarshipId, sp.PilotId });

            modelBuilder.Entity<StarshipFilm>()
                .HasKey(sf => new { sf.StarshipId, sf.FilmId });

            base.OnModelCreating(modelBuilder);
        }
    }

}

