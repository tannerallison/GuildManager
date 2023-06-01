using System.Security.Claims;
using GuildManager.Controllers;
using GuildManager.Data;
using GuildManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace GuildManager.Tests;

public class Tests
{
    private MyMinionsController _controller;
    private Mock<GMContext> _mockContext;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<GMContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _mockContext = new Mock<GMContext>(options);
        _controller = new MyMinionsController(_mockContext.Object);
    }

    [Test]
    public async Task GetMinions_WithAuthorizedUser_ReturnsMinions()
    {
        // Arrange
        var player = new Player();
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, "username"),
            new Claim(ClaimTypes.NameIdentifier, player.Id.ToString())
        }, "mock"));

        var minions = new List<Minion>
        {
            new() { BossId = player.Id },
            new() { BossId = player.Id }
        };
        var mockDbSet = minions.AsQueryable().BuildMockDbSet();

        _mockContext.SetupProperty(c => c.Minions, mockDbSet.Object);

        // Act
        var result = await _controller.GetMinions();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        var resultList = okResult.Value as List<Minion>;
        Assert.That(resultList.Count, Is.EqualTo(2));
    }
}
