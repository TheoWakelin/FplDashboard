using FplDashboard.API.Features.Players;
using FplDashboard.API.Tests.Infrastructure;
using FplDashboard.DataModel.Models;
using Microsoft.EntityFrameworkCore;

namespace FplDashboard.API.Tests.Features.Players;

public class PlayersControllerIntegrationTests(DatabaseFixture fixture) : BaseIntegrationTest(fixture)
{
    private const string PlayersPagedEndpoint = "/players/paged";
    private const string TeamsEndpoint = "/players/teams";
    private const int GoalKeeper = (int)Position.GoalKeeper;
    
    private ApiTestClient ApiClient => new(Client);

    [Fact]
    public async Task GetPagedPlayers_DefaultPaginationAndFilters_ReturnsAllPlayers()
    {
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, new PlayerFilterRequest());
        Assert.Equal(TestConfiguration.TestData.DefaultPlayerCount, result.Count);
    }

    [Fact]
    public async Task GetPagedPlayers_CustomPageSize_ReturnsCorrectCount()
    {
        var request = new PlayerFilterRequest { PageSize = 2 };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetPagedPlayers_LargePageNumber_ReturnsEmpty()
    {
        var request = new PlayerFilterRequest { Page = 10, PageSize = 2 };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPagedPlayers_FilterByTeamId_ReturnsOnlyTeamPlayers()
    {
        var team = Fixture.SeededTeams[0];
        var request = new PlayerFilterRequest { TeamIds = [team.Id] };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.All(result, p => Assert.Equal(team.Name, p.TeamName));
    }
    
    [Fact]
    public async Task GetPagedPlayers_MultipleTeamIds_ReturnsPlayersFromAllTeams()
    {
        var teamIds = Fixture.SeededTeams.Select(t => t.Id).ToList();
        var request = new PlayerFilterRequest { TeamIds = teamIds };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.Equal(TestConfiguration.TestData.DefaultPlayerCount, result.Count);
    }

    [Fact]
    public async Task GetPagedPlayers_FilterByPositionId_ReturnsOnlyPositionPlayers()
    {
        var request = new PlayerFilterRequest { PositionIds = [GoalKeeper] };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.All(result, p => Assert.Equal(GoalKeeper, p.Position));
    }
    
    [Fact]
    public async Task GetPagedPlayers_MultiplePositionIds_ReturnsPlayersFromAllPositions()
    {
        var positionIds = new List<int> { GoalKeeper, (int)Position.Defender };
        var request = new PlayerFilterRequest { PositionIds = positionIds };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        var expectedCount = TestConfiguration.TestData.SeededPlayers.Count(p => p.Position is Position.GoalKeeper or Position.Defender);
        Assert.Equal(expectedCount, result.Count);
    }

    [Fact]
    public async Task GetPagedPlayers_FilterByPlayerName_ReturnsPartialMatches()
    {
        var request = new PlayerFilterRequest { PlayerName = "li" };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, p => p.PlayerName == "Alice");
        Assert.Contains(result, p => p.PlayerName == "Charlie");
    }

    [Fact]
    public async Task GetPagedPlayers_CaseInsensitivePlayerName_ReturnsMatches()
    {
        var request = new PlayerFilterRequest { PlayerName = "ALICE" };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.Contains(result, p => p.PlayerName.Equals("Alice", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task GetPagedPlayers_FilterByMinMinutes_ReturnsOnlyQualifyingPlayers()
    {
        var request = new PlayerFilterRequest { MinMinutes = 90 };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.All(result, p => Assert.True(p.Minutes >= 90));
    }

    [Theory]
    [InlineData("PlayerName")]
    [InlineData("TotalPoints")] 
    [InlineData("TeamName")]
    public async Task GetPagedPlayers_Sorting_WorksForKeyColumns(string orderBy)
    {
        var request = new PlayerFilterRequest { OrderBy = orderBy, OrderDir = "ASC" };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        var sorted = result.OrderBy(p => GetComparableValue(p, orderBy)).ToList();
        Assert.Equal(sorted, result);
    }

    [Fact]
    public async Task GetPagedPlayers_Sorting_DescendingOrder()
    {
        var request = new PlayerFilterRequest { OrderBy = "TotalPoints", OrderDir = "DESC" };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        var sorted = result.OrderByDescending(p => p.TotalPoints).ToList();
        Assert.Equal(sorted, result);
    }
    
    [Fact]
    public async Task GetPagedPlayers_CombinedFilters_ReturnsCorrectResults()
    {
        var filteredTeam = Fixture.SeededTeams[0];
        var request = new PlayerFilterRequest 
        { 
            TeamIds = [filteredTeam.Id],
            PositionIds = [GoalKeeper],
            MinMinutes = 0
        };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        
        foreach (var player in result)
        {
            Assert.Equal(filteredTeam.Name, player.TeamName);
            Assert.Equal(GoalKeeper, player.Position);
        }
    }

    [Fact]
    public async Task GetPagedPlayers_WithSqlInjectionAttempt_PreventsSqlInjection()
    {
        var request = new PlayerFilterRequest { PlayerName = "Alice'; DROP TABLE Players;--" };
        var result = await ApiClient.PostAndExpectSuccessAsync<List<PlayerPagedDto>>(PlayersPagedEndpoint, request);
        Assert.Empty(result);
        Assert.Equal(4, await Fixture.DbContext.Players.CountAsync()); // Table still exists
    }
    
    [Fact]
    public async Task GetAllTeams_ReturnsAllTeams()
    {
        var result = await ApiClient.GetAndExpectSuccessAsync<List<TeamListDto>>(TeamsEndpoint);
        Assert.Equal(TestConfiguration.TestData.DefaultTeamCount, result.Count);
        Assert.All(result, team => Assert.NotEmpty(team.Name));
    }

    private static object? GetComparableValue(object obj, string propertyName)
    {
        var prop = obj.GetType().GetProperty(propertyName);
        if (prop == null) return null;
        var value = prop.GetValue(obj);
        if (value == null) return null;
        var type = prop.PropertyType;
        if (type.IsEnum) return (int)value;
        return value;
    }
}
