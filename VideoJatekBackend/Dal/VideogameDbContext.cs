using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;
using VideoJatekBackend.Models;
using YamlDotNet.Core.Tokens;

namespace VideoJatekBackend.Dal
{
    public class VideogameDbContext : DbContext
    {
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Videogame> Videogames { get; set; }

        public VideogameDbContext()
        {

        }

        public VideogameDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Videogame>()
                .HasOne(e => e.Publisher)
                .WithMany(e => e.Videogames)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
