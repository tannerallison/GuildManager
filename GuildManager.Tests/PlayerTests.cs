using FluentAssertions;
using GuildManager.Controllers;
using GuildManager.Data;
using GuildManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace GuildManager.Tests;

public class PlayerTest : BaseUnitTest
{
    private PlayersController _controller;
    private Mock<GMContext> _mockContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<GMContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _mockContext = new Mock<GMContext>(options);
        _controller = new PlayersController(_mockContext.Object);
    }

    [Test]
    public async Task GetPlayersReturnsAllPlayers()
    {
        // Arrange
        var player = SetUpAuthenticatedPlayer(_controller);

        var players = new List<Player>
        {
            new() { Id = player.Id },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };
        var mockDbSet = players.AsQueryable().BuildMockDbSet();

        _mockContext.SetupProperty(c => c.Players, mockDbSet.Object);

        // Act
        var result = await _controller.GetPlayers();

        // Assert
        result.Result.Should().BeAssignableTo<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var resultList = okResult.Value as IEnumerable<Player>;
        resultList.Count().Should().Be(3);
    }
}
