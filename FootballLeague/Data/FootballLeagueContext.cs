namespace FootballLeague.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FootballLeagueContext : DbContext
    {
        public DbSet<Team> Teams { get; set; }

        public DbSet<Match> Matches { get; set; }

        public FootballLeagueContext(DbContextOptions<FootballLeagueContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Team>()
                .HasIndex(u => u.Name)
                .IsUnique();

            builder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany(t => t.HomeMatches)
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany(t => t.AwayMatches)
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
