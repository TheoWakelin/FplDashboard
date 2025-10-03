using AutoFixture;
using FplDashboard.DataModel.Models;
using Fixture = AutoFixture.Fixture;

namespace FplDashboard.API.Tests.Infrastructure;

public static class TestDataSeedHelper
{
    public static List<Team> GetTeamsFromNames(this Fixture fixture, params string[] names)
    {
        return names.Select(name => fixture.Build<Team>()
            .With(t => t.Name, name)
            .Without(t => t.Players)
            .Without(t => t.TeamGameWeekData)
            .Without(t => t.HomeFixtures)
            .Without(t => t.AwayFixtures)
            .Create())
            .ToList();
    }
    
    public static List<Player> GetPlayersFromSeededPlayers(this Fixture fixture, SeededPlayer[] players, int teamId)
    {
        return players.Select(player => fixture.Build<Player>()
                .With(p => p.TeamId, teamId)
                .With(p => p.WebName, player.Name)
                .With(p => p.Position, player.Position)
                .With(p => p.Status, "a")
                .Without(p => p.Team)
                .Without(p => p.News)
                .Without(p => p.PlayerGameWeekData)
                .Create())
            .ToList();
    }
}
