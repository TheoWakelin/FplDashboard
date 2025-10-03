// using System.Data.Common;
// using FplDashboard.API.Features.Dashboard;
// using FplDashboard.API.Features.Shared;
// using FplDashboard.DataModel;
// using FplDashboard.DataModel.Models;
// using Microsoft.Data.Sqlite;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using FplDashboard.API.IntegrationTests.Infrastructure;
// using AutoFixture;
// using Microsoft.AspNetCore.Mvc.Testing;
//
// namespace FplDashboard.API.IntegrationTests;
//
// public class DashboardQueriesTests : BaseIntegrationTest
// {
//     private readonly HttpClient _client;
//     private readonly Fixture _fixture;
//
//     public DashboardQueriesTests(DatabaseFixture fixture) : base(fixture)
//     {
//         _client = Factory.CreateClient();
//         _fixture = new Fixture();
//     }
//
//     [Fact]
//     public async Task GetDashboardData_ReturnsExpectedResults_WhenDataIsSeeded()
//     {
//         // Seed minimal valid data using fixture
//         var team = _fixture.Build<Team>().Without(t => t.Players).Create();
//         var player = _fixture.Build<Player>().With(p => p.TeamId, team.Id).Create();
//         var gameweek = _fixture.Create<GameWeek>();
//         var playerNews = _fixture.Build<PlayerNews>().With(n => n.PlayerId, player.Id).Create();
//         var teamGameWeekData = _fixture.Build<TeamGameWeekData>().With(tg => tg.TeamId, team.Id).With(tg => tg.GameWeekId, gameweek.Id).Create();
//         var fixtureEntity = _fixture.Build<Fixture>().With(f => f.GameweekId, gameweek.Id).With(f => f.AwayTeamId, team.Id).With(f => f.HomeTeamId, team.Id).Without(f => f.GameWeek).Create();
//
//         await Fixture.DbContext.Teams.AddAsync(team);
//         await Fixture.DbContext.Players.AddAsync(player);
//         await Fixture.DbContext.GameWeeks.AddAsync(gameweek);
//         await Fixture.DbContext.PlayerNews.AddAsync(playerNews);
//         await Fixture.DbContext.TeamGameWeekData.AddAsync(teamGameWeekData);
//         await Fixture.DbContext.AddAsync(fixtureEntity);
//         await Fixture.DbContext.SaveChangesAsync();
//
//         // Act
//         var response = await _client.GetAsync("/dashboard");
//         response.EnsureSuccessStatusCode();
//         var result = await response.Content.ReadAsStringAsync();
//
//         // Assert
//         Assert.False(string.IsNullOrEmpty(result));
//         // Further asserts can be added for each returned property
//     }
// }