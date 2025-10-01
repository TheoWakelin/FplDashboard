using Microsoft.EntityFrameworkCore;
using FplDashboard.DataModel.Models;

namespace FplDashboard.DataModel;

public class FplDashboardDbContext(DbContextOptions<FplDashboardDbContext> options) : DbContext(options)
{
    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<GameWeek> GameWeeks { get; set; }
    public DbSet<PlayerGameWeekData> PlayerGameWeekData { get; set; }
    public DbSet<TeamGameWeekData> TeamGameWeekData { get; set; }
    public DbSet<PlayerNews> PlayerNews { get; set; }
    public DbSet<Fixture> Fixtures { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite keys
        modelBuilder.Entity<GameWeek>()
            .HasIndex(gw => new { Number = gw.GameWeekNumber, gw.YearSeasonStarted })
            .IsUnique();
        modelBuilder.Entity<PlayerGameWeekData>()
            .HasKey(pg => new { pg.PlayerId, pg.GameWeekId });
        modelBuilder.Entity<TeamGameWeekData>()
            .HasKey(tg => new { tg.TeamId, tg.GameWeekId });
        modelBuilder.Entity<PlayerNews>()
            .HasKey(pn => new { pn.PlayerId, pn.NewsAdded });

        // Relationships
        modelBuilder.Entity<Player>()
            .HasOne(p => p.Team)
            .WithMany(t => t.Players)
            .HasForeignKey(p => p.TeamId);

        modelBuilder.Entity<PlayerGameWeekData>()
            .HasOne(pg => pg.Player)
            .WithMany(p => p.PlayerGameWeekData)
            .HasForeignKey(pg => pg.PlayerId);

        modelBuilder.Entity<PlayerGameWeekData>()
            .HasOne(pg => pg.GameWeek)
            .WithMany(gw => gw.PlayerGameWeekData)
            .HasForeignKey(pg => pg.GameWeekId);

        modelBuilder.Entity<TeamGameWeekData>()
            .HasOne(tg => tg.Team)
            .WithMany(t => t.TeamGameWeekData)
            .HasForeignKey(tg => tg.TeamId);

        modelBuilder.Entity<TeamGameWeekData>()
            .HasOne(tg => tg.GameWeek)
            .WithMany(gw => gw.TeamGameWeekData)
            .HasForeignKey(tg => tg.GameWeekId);

        modelBuilder.Entity<PlayerNews>()
            .HasOne(pn => pn.Player)
            .WithMany(p => p.News)
            .HasForeignKey(pn => pn.PlayerId);

        modelBuilder.Entity<Fixture>()
            .HasOne(f => f.GameWeek)
            .WithMany(gw => gw.Fixtures)
            .HasForeignKey(f => f.GameweekId);
        modelBuilder.Entity<Fixture>()
            .HasOne(f => f.AwayTeam)
            .WithMany()
            .HasForeignKey(f => f.AwayTeamId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Fixture>()
            .HasOne(f => f.HomeTeam)
            .WithMany()
            .HasForeignKey(f => f.HomeTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        // Enum conversions
        modelBuilder.Entity<Player>()
            .Property(p => p.Position)
            .HasConversion<int>();

        modelBuilder.Entity<GameWeek>()
            .Property(gw => gw.Status)
            .HasConversion<int>();
    }
}
