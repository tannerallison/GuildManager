using System.Linq.Expressions;
using System.Security.Claims;
using GuildManager.Controllers;
using GuildManager.DAL;
using GuildManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace GuildManager.Tests;

public class Tests
{
    private MyMinionsController _controller;
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private Mock<IRepository<Minion>> _mockMinionRepository;

    [SetUp]
    public void Setup()
    {
        _mockMinionRepository = new Mock<IRepository<Minion>>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockUnitOfWork.Setup(u => u.GetRepository<Minion>()).Returns(_mockMinionRepository.Object);
        _controller = new MyMinionsController(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetMinions_WithAuthorizedUser_ReturnsMinions()
    {
        // Arrange
        var player = new Player();
        _controller.ControllerContext = new ControllerContext();
        _controller.ControllerContext.HttpContext = new DefaultHttpContext();
        _controller.ControllerContext.HttpContext.Items["Player"] = player;

        var minions = new List<Minion>
        {
            new() { BossId = player.Id },
            new() { BossId = player.Id }
        };

        _mockMinionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Minion, bool>>>(), null, ""))
            .ReturnsAsync(minions)
            .Verifiable("Get was not called");

        // Act
        var result = await _controller.GetMinions();

        // Assert
        _mockMinionRepository.Verify();
        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        var resultList = okResult.Value as List<Minion>;
        Assert.That(resultList.Count, Is.EqualTo(2));
    }
}
