using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PersonalProjectApi.Controllers;
using PersonalProjectApi.Models.Domain;
using PersonalProjectApi.Models.DTO.CardDTOs;
using PersonalProjectApi.Repositories.ActivityLogRepositories;
using PersonalProjectApi.Repositories.BoardRepository;
using PersonalProjectApi.Repositories.CardsRepositories;
using PersonalProjectApi.Repositories.ListRepositories;
using Xunit;

public class BoardServiceTests
{
    private readonly Mock<ICardsRepository> _mockRepo;
    private readonly CardsController _service;

    public BoardServiceTests()
    {
        // Створюємо фейковий репозиторій, щоб не лізти в БД
        _mockRepo = new Mock<ICardsRepository>();
        _service = new CardsController(_mockRepo.Object);
    }

    [Fact]
    public async Task DeleteCardAsync_ReturnsTrue_WhenCardExists()
    {
        // Arrange: налаштовуємо мок, ніби картка успішно видалилась
        var cardId = Guid.NewGuid();
        _mockRepo.Setup(repo => repo.DeleteCardAsync(cardId)).ReturnsAsync(true);

        // Act: викликаємо твій публічний метод
        var result = await _service.DeleteCard(cardId);

        // Assert: перевіряємо, що повернувся саме OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result);

        // Assert: перевіряємо, чи повернувся очікуваний результат
        Assert.Equal("Card was deleted succesfully.", okResult.Value);
        _mockRepo.Verify(repo => repo.DeleteCardAsync(cardId), Times.Once); // Перевіряємо, чи репо взагалі смикали
    }

    [Fact]
    public async Task CreateCard_ReturnsOk_WhenDataIsValid()
    {
        // Arrange
        var newCardDto = new AddCardRequestDto { Title = "New Task", Priority = TaskPriority.High };
        var expectedCard = new Card { Id = Guid.NewGuid(), Title = "New Task" };

        // Кажемо моку: якщо викличуть метод створення з БУДЬ-ЯКОЮ карткою, повертай наш expectedCard
        _mockRepo.Setup(repo => repo.CreateCardAsync(It.IsAny<Card>()))
                 .ReturnsAsync(expectedCard);

        // Act
        var result = await _service.CreateCard(newCardDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task UpdateCard_ReturnsOk_WhenCardIsUpdated()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var updateDto = new UpdateCardDto { Title = "Updated Task" };
        var updatedCard = new Card { Id = cardId, Title = "Updated Task" };

        // Юзаємо It.IsAny для переданих даних, але айдішник фіксуємо
        _mockRepo.Setup(repo => repo.UpdateCardAsync(cardId, It.IsAny<Card>()))
                 .ReturnsAsync(updatedCard); // Або true, залежить від твого ICardsRepository

        // Act
        var result = await _service.UpdateCard(cardId, updateDto); // Знову ж таки, вписуй свій метод

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    // Unit tests

    [Fact]
    public async Task GetAllBoards_ReturnsOk_WithData()
    {
        // Arrange: мокаємо репозиторій дошок
        var mockRepo = new Mock<IBoardRepository>();
        var controller = new BoardsController(mockRepo.Object);

        var expectedBoards = new List<Board> { new Board { Id = Guid.NewGuid(), Title = "Test Board" } };
        mockRepo.Setup(repo => repo.GetAllBoardsAsync()).ReturnsAsync(expectedBoards);

        // Act
        var result = await controller.GetAllBoards();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task GetBoardLogs_ReturnsOk_WithLogs()
    {
        // Arrange: мокаємо репозиторій логів
        var mockRepo = new Mock<IActivityLogRepository>();
        var controller = new LogsController(mockRepo.Object);
        var boardId = Guid.NewGuid();

        var expectedLogs = new List<ActivityLog> { new ActivityLog { Id = Guid.NewGuid(), Description = "Test log" } };

        // Уважно з аргументами! Ти сам писав skip: 0, take: 10
        mockRepo.Setup(repo => repo.GetLogsAsync(boardId, null, null, 0, 10)).ReturnsAsync(expectedLogs);

        // Act
        var result = await controller.GetBoardLogs(boardId, 0, 10);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task DeleteList_ReturnsOk_WhenSuccessful()
    {
        // Arrange: мокаємо репозиторій списків
        var mockRepo = new Mock<IListsRepository>();
        var controller = new ListsController(mockRepo.Object);
        var listId = Guid.NewGuid();

        mockRepo.Setup(repo => repo.DeleteListAsync(listId)).ReturnsAsync(true);

        // Act
        var result = await controller.DeleteList(listId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}