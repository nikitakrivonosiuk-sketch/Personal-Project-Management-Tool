using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PersonalProjectApi.Controllers;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Models.Enums;
using PersonalProjectApi.Repositories.ActivityLogRepositories;
using PersonalProjectApi.Services;
using System.Drawing.Printing;
using Xunit;

public class BoardServiceTests
{
    private readonly Mock<CardsService> _mockService;
    private readonly CardsController _controller;

    public BoardServiceTests()
    {
        _mockService = new Mock<CardsService>();
        var mockLogger = Mock.Of<ILogger<CardsController>>();
        _controller = new CardsController(_mockService.Object, mockLogger);
    }

    [Fact]
    public async Task DeleteCardAsync_ReturnsTrue_WhenCardExists()
    {
        var cardId = Guid.NewGuid();
        _mockService.Setup(s => s.DeleteCardAsync(cardId)).ReturnsAsync("Card was deleted succesfully.");

        var result = await _controller.DeleteCard(cardId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Card was deleted succesfully.", okResult.Value);
        _mockService.Verify(s => s.DeleteCardAsync(cardId), Times.Once);
    }

    [Fact]
    public async Task CreateCard_ReturnsOk_WhenDataIsValid()
    {
        var newCardDto = new AddCardRequestDto { Title = "New Task", Priority = Priority.High };
        var expectedDto = new CardDto { Id = Guid.NewGuid(), Title = "New Task" };

        _mockService.Setup(s => s.CreateCardAsync(It.IsAny<AddCardRequestDto>()))
                 .ReturnsAsync(expectedDto);

        var result = await _controller.CreateCard(newCardDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task UpdateCard_ReturnsOk_WhenCardIsUpdated()
    {
        var cardId = Guid.NewGuid();
        var updateDto = new UpdateCardDto { Title = "Updated Task" };
        var expectedDto = new CardDto { Id = cardId, Title = "Updated Task" };

        _mockService.Setup(s => s.UpdateCardAsync(cardId, It.IsAny<UpdateCardDto>()))
                 .ReturnsAsync(expectedDto);

        var result = await _controller.UpdateCard(cardId, updateDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetAllBoards_ReturnsOk_WithData()
    {
        var mockBoardService = new Mock<BoardService>();
        var mockLogger = Mock.Of<ILogger<BoardsController>>();
        var controller = new BoardsController(mockBoardService.Object, mockLogger);

        var expectedBoards = new List<Board> { new Board { Id = Guid.NewGuid(), Title = "Test Board" } };

        mockBoardService.Setup(s => s.GetAllBoards(1, 8)).ReturnsAsync(expectedBoards);

        var result = await controller.GetAllBoards();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetBoardLogs_ReturnsOk_WithLogs()
    {
        var mockRepo = new Mock<IActivityLogRepository>();
        var mockLogger = Mock.Of<ILogger<LogsController>>();
        var controller = new LogsController(mockRepo.Object, mockLogger);
        var boardId = Guid.NewGuid();

        var expectedLogs = new List<ActivityLog> { new ActivityLog { Id = Guid.NewGuid(), Description = "Test log" } };

        mockRepo.Setup(repo => repo.GetLogsAsync(boardId, null, null, 0, 10)).ReturnsAsync(expectedLogs);

        var result = await controller.GetLogs(boardId, null, null, 0, 10);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task DeleteList_ReturnsOk_WhenSuccessful()
    {
        var mockListService = new Mock<ListService>();
        var mockLogger = Mock.Of<ILogger<ListsController>>();
        var controller = new ListsController(mockListService.Object, mockLogger);
        var listId = Guid.NewGuid();

        mockListService.Setup(s => s.DeleteListAsync(listId)).ReturnsAsync("List was deleted successfully.");

        var result = await controller.DeleteList(listId);

        Assert.IsType<OkObjectResult>(result);
    }
}