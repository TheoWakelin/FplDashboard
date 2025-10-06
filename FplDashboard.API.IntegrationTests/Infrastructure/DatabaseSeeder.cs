using DomainFixture = FplDashboard.DataModel.Models.Fixture;
using AutoFixture;
using FplDashboard.API.IntegrationTests.Infrastructure.Models;
using FplDashboard.DataModel;
using FplDashboard.DataModel.Models;
using Fixture = AutoFixture.Fixture;

namespace FplDashboard.API.IntegrationTests.Infrastructure;

public class DatabaseSeeder(FplDashboardDbContext dbContext, Fixture fixture)
{
    public async Task<DashboardSeededData> SeedAllTestDataAsync()
    {
        var seededTeams = await SeedTeamsAsync();
        var seededGameWeek = await SeedCurrentGameWeekAsync();
        var futureGameWeeks = await SeedFutureGameWeeksAsync();
        var seededPlayers = await SeedPlayersAsync(seededTeams[0].Id);

        // Prepare all entities to add
        var playerGameWeekData = GetPlayerGameWeekData(seededPlayers, seededGameWeek.Id);
        var teamGameWeekData = GetTeamGameWeekData(seededTeams, seededGameWeek.Id);
        var fixtures = GetFixtures(seededTeams, futureGameWeeks);
        var playerNews = GetPlayerNews(seededPlayers);

        // Add all entities to context
        await Task.WhenAll(
            dbContext.PlayerGameWeekData.AddRangeAsync(playerGameWeekData),
            dbContext.TeamGameWeekData.AddRangeAsync(teamGameWeekData),
            dbContext.Fixtures.AddRangeAsync(fixtures),
            dbContext.PlayerNews.AddRangeAsync(playerNews)
        );
        await dbContext.SaveChangesAsync();

        return new DashboardSeededData
        {
            Teams = seededTeams,
            GameWeek = seededGameWeek,
            Players = seededPlayers,
            FutureGameWeeks = futureGameWeeks,
            Fixtures = fixtures,
            PlayerNews = playerNews,
            TeamGameWeekData = teamGameWeekData
        };
    }

    private async Task<List<Team>> SeedTeamsAsync()
    {
        var teams = fixture.GetTeamsFromNames(TestConfiguration.TestData.DefaultTeamNames);
        await dbContext.Teams.AddRangeAsync(teams);
        await dbContext.SaveChangesAsync();
        return teams;
    }

    private async Task<GameWeek> SeedCurrentGameWeekAsync()
    {
        var gameWeek = fixture.Build<GameWeek>()
            .With(gw => gw.GameWeekNumber, 1)
            .With(gw => gw.YearSeasonStarted, 2025)
            .With(gw => gw.Status, GameWeekStatus.Current)
            .OmitAutoProperties()
            .Create();
        await dbContext.GameWeeks.AddAsync(gameWeek);
        await dbContext.SaveChangesAsync();
        return gameWeek;
    }

    private async Task<List<GameWeek>> SeedFutureGameWeeksAsync()
    {
        var futureGameWeeks = new List<GameWeek>();
        for (var i = 2; i <= 6; i++)
        {
            var gw = fixture.Build<GameWeek>()
                .With(gw => gw.GameWeekNumber, i)
                .With(gw => gw.YearSeasonStarted, 2025)
                .With(gw => gw.Status, GameWeekStatus.Future)
                .OmitAutoProperties()
                .Create();
            futureGameWeeks.Add(gw);
        }
        await dbContext.GameWeeks.AddRangeAsync(futureGameWeeks);
        await dbContext.SaveChangesAsync();
        return futureGameWeeks;
    }

    private async Task<List<Player>> SeedPlayersAsync(int teamId)
    {
        var players = fixture.GetPlayersFromSeededPlayers(TestConfiguration.TestData.SeededPlayers, teamId);
        await dbContext.Players.AddRangeAsync(players);
        await dbContext.SaveChangesAsync();
        return players;
    }

    private IEnumerable<PlayerGameWeekData> GetPlayerGameWeekData(List<Player> players, int gameWeekId)
    {
        return players.Select(player => fixture.Build<PlayerGameWeekData>()
            .With(pg => pg.PlayerId, player.Id)
            .With(pg => pg.GameWeekId, gameWeekId)
            .Without(pg => pg.GameWeek)
            .Without(pg => pg.Player)
            .Create());
    }

    private List<TeamGameWeekData> GetTeamGameWeekData(List<Team> allTeams, int gameWeekId)
    {
        return allTeams.Select(team => {
            var strengths = TestConfiguration.TestData.SeededTeamStrengths
                .FirstOrDefault(s => s.TeamName == team.Name);
            return fixture.Build<TeamGameWeekData>()
                .With(tgd => tgd.TeamId, team.Id)
                .With(tgd => tgd.GameWeekId, gameWeekId)
                .With(tgd => tgd.StrengthAttackHome, strengths?.AttackHome ?? 3)
                .With(tgd => tgd.StrengthDefenceHome, strengths?.DefenceHome ?? 3)
                .With(tgd => tgd.StrengthAttackAway, strengths?.AttackAway ?? 3)
                .With(tgd => tgd.StrengthDefenceAway, strengths?.DefenceAway ?? 3)
                .Without(tgd => tgd.Team)
                .Without(tgd => tgd.GameWeek)
                .Create();
        }).ToList();
    }

    private static List<DomainFixture> GetFixtures(List<Team> allTeams, List<GameWeek> futureGameWeeks)
    {
        var fixtures = new List<DomainFixture>();
        var random = new Random();
        foreach (var gameWeek in futureGameWeeks)
        {
            var shuffledTeams = allTeams.OrderBy(_ => random.Next()).ToList();
            for (var i = 0; i < shuffledTeams.Count - 1; i += 2)
            {
                if (shuffledTeams.Count <= i + 1) continue;
                var fixtureEntity = new DomainFixture
                {
                    GameweekId = gameWeek.Id,
                    HomeTeamId = shuffledTeams[i].Id,
                    AwayTeamId = shuffledTeams[i + 1].Id,
                    KickoffTime = DateTime.UtcNow.AddDays((gameWeek.GameWeekNumber - 1) * 7),
                    Finished = false
                };
                fixtures.Add(fixtureEntity);
            }
        }
        return fixtures;
    }

    private List<PlayerNews> GetPlayerNews(List<Player> players)
    {
        return players.Take(2)
            .Select(player => fixture.Build<PlayerNews>()
                .With(pn => pn.PlayerId, player.Id)
                .With(pn => pn.News, $"Test news for {player.WebName}")
                .With(pn => pn.NewsAdded, DateTime.UtcNow.AddHours(-player.Id))
                .Without(pn => pn.Player)
                .Create())
            .ToList();
    }
}