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

        // Assert - Should cache popular combination
        MockCacheService.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<IEnumerable<PlayerPagedDto>>()), Times.Once);
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
        MockCacheService.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<IEnumerable<PlayerPagedDto>>()), Times.Never);
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
        MockCacheService.Verify(m => m.Set(It.IsAny<string>(), It.IsAny<PlayerPagedDto>()), Times.Never);
    }

    public static IEnumerable<object[]> CacheableFilterTestData =>
        new List<object[]>
        {
            new object[] { new PlayerFilterRequest() },
            new object[] { new PlayerFilterRequest { TeamIds = [1] } },
            new object[] { new PlayerFilterRequest { TeamIds = [1, 2] } },
            new object[] { new PlayerFilterRequest { PositionIds = [(int)Position.Forward] } },
            new object[] { new PlayerFilterRequest { PositionIds = [(int)Position.Forward, (int)Position.Midfielder] } }
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