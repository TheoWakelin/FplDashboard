using Dapper;
using FplDashboard.API.Features.Players;
using FplDashboard.API.Features.Players.Models;
using FplDashboard.API.Features.Shared;
using FplDashboard.API.UnitTests.Infrastructure;
using FplDashboard.DataModel.Models;
using Moq;
using Moq.Dapper;

namespace FplDashboard.API.UnitTests.Features.Players;

public class PlayersQueriesTests : QueriesUsingGeneralTestsBase
{
    private readonly PlayersQueries _sut;
    
    public PlayersQueriesTests()
    {
        _sut = new PlayersQueries(MockConnectionFactory.Object, MockGeneralQueries.Object, MockCacheService.Object);
    }

    [Theory]
    [MemberData(nameof(CacheableFilterTestData))]
    public async Task GetPlayersAsync_CachesOnlySelectiveFilters(PlayerFilterRequest filters)
    {
        // Arrange
        var players = CreateTestPlayers();

        // Act
        await _sut.GetPagedPlayersAsync(filters, CancellationToken.None);

        // Assert
        var cacheKey = filters.GenerateCacheKey();
        VerifyCacheSet<IEnumerable<PlayerPagedDto>>(cacheKey);
    }

    [Fact]
    public async Task GetPlayersAsync_ReturnsCachedData_WhenCacheHit()
    {
        // Arrange
        var filters = new PlayerFilterRequest { PositionIds = [(int)Position.Forward] };
        var cachedPlayers = CreateTestPlayers();
        
        MockCacheService.Setup(m => m.Get<IEnumerable<PlayerPagedDto>>(It.IsAny<string>()))
            .Returns(cachedPlayers);

        // Act
        var result = await _sut.GetPagedPlayersAsync(filters, CancellationToken.None);

        // Assert
        Assert.Equal(cachedPlayers, result);
        VerifyCacheNotSet<IEnumerable<PlayerPagedDto>>(It.IsAny<string>());
    }

    [Fact]
    public async Task GetPlayersAsync_SkipsCacheForComplexFilters()
    {
        // Arrange - Complex filter combination
        var complexFilters = new PlayerFilterRequest 
        { 
            PositionIds = [(int)Position.Forward], 
            TeamIds = [1, 2, 3],
            PlayerName = "John",
            MinMinutes = 50
        };
        CreateTestPlayers();
    
        // Act
        await _sut.GetPagedPlayersAsync(complexFilters, CancellationToken.None);

        // Assert - Should not cache complex combination
        VerifyCacheNotSet<PlayerPagedDto>(It.IsAny<string>());
    }

    [Fact]
    public async Task GetAllTeamsAsync_ReturnsCachedData_WhenCacheHit()
    {
        // Arrange
        var cachedTeams = new List<TeamListDto> { new() { Id = 1, Name = "TeamA" } };
        MockCacheService.Setup(m => m.Get<List<TeamListDto>>(CacheKeys.PlayerTeams)).Returns(cachedTeams);

        // Act
        var result = await _sut.GetAllTeamsAsync(CancellationToken.None);

        // Assert
        Assert.Equal(cachedTeams, result);
        VerifyConnectionNotCreated();
        VerifyCacheNotSet<List<TeamListDto>>(CacheKeys.PlayerTeams);
    }

    [Fact]
    public async Task GetAllTeamsAsync_QueriesDatabaseAndCaches_WhenCacheMiss()
    {
        // Arrange
        MockCacheService.Setup(m => m.Get<List<TeamListDto>>(CacheKeys.PlayerTeams)).Returns((List<TeamListDto>?)null);
        var dbTeams = new List<TeamListDto> { new() { Id = 2, Name = "TeamB" } };
        MockDbConnection.SetupDapperAsync(c => c.QueryAsync<TeamListDto>(It.IsAny<CommandDefinition>())).ReturnsAsync(dbTeams);

        // Act
        var result = await _sut.GetAllTeamsAsync(CancellationToken.None);

        // Assert
        Assert.Single(result);
        Assert.Equal(dbTeams[0].Id, result[0].Id);
        Assert.Equal(dbTeams[0].Name, result[0].Name);
        VerifyCacheSet<List<TeamListDto>>(CacheKeys.PlayerTeams);
        VerifyConnectionCreated();
    }

    public static TheoryData<PlayerFilterRequest> CacheableFilterTestData => new()
    {
        new PlayerFilterRequest(),
        new PlayerFilterRequest { TeamIds = [1] },
        new PlayerFilterRequest { TeamIds = [1, 2] },
        new PlayerFilterRequest { PositionIds = [(int)Position.Forward] },
        new PlayerFilterRequest { PositionIds = [(int)Position.Forward, (int)Position.Midfielder] }
    };

    private List<PlayerPagedDto> CreateTestPlayers()
    {
        var players = new List<PlayerPagedDto> 
        { 
            new() { PlayerName = "Test Player 1" },
            new() { PlayerName = "Test Player 2" }
        };
        
        MockDbConnection.SetupDapperAsync(c => c.QueryAsync<PlayerPagedDto>(It.IsAny<CommandDefinition>()))
            .ReturnsAsync(players);

        return players;
    }
}