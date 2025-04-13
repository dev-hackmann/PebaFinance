using Moq;
using Xunit;
using PebaFinance.Application.Commands;
using PebaFinance.Application.Exceptions;
using PebaFinance.Application.Handlers.ExpensesHandlers;
using PebaFinance.Application.Interfaces;
using PebaFinance.Domain.Models;

namespace PebaFinance.UnitTests.Application;

public class ExpenseCommandHandlersTests
{
    private readonly Mock<IExpensesRepository> _mockRepository;

    public ExpenseCommandHandlersTests()
    {
        _mockRepository = new Mock<IExpensesRepository>();
    }

    [Fact]
    public async Task CreateExpenseCommandHandler_ShouldReturnId_WhenSuccessful()
    {
        // Arrange
        const int expectedId = 1;
        var command = new CreateExpenseCommand("Test Expense", 100.00m, DateTime.Now);

        _mockRepository
            .Setup(r => r.ExistsByDescriptionInTheSameMonthAsync(command.Description, command.Date))
            .ReturnsAsync(false);

        _mockRepository
            .Setup(r => r.AddAsync(It.IsAny<Expense>()))
            .ReturnsAsync(expectedId);

        var handler = new CreateExpensesCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(expectedId, result);
        _mockRepository.Verify(r => r.AddAsync(It.Is<Expense>(e =>
            e.Description == command.Description &&
            e.Value == command.Value &&
            e.Date == command.Date)), Times.Once);
    }

    [Fact]
    public async Task CreateExpenseCommandHandler_ShouldThrowDuplicateDescriptionException_WhenDuplicateExists()
    {
        // Arrange
        var command = new CreateExpenseCommand("Duplicate Expense", 100.00m, DateTime.Now);

        _mockRepository
            .Setup(r => r.ExistsByDescriptionInTheSameMonthAsync(command.Description, command.Date))
            .ReturnsAsync(true);

        var handler = new CreateExpensesCommandHandler(_mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateDescriptionException>(() =>
            handler.Handle(command, CancellationToken.None));

        _mockRepository.Verify(r => r.AddAsync(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task UpdateExpenseCommandHandler_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var existingExpense = new Expense
        {
            Id = 1,
            Description = "Original Description",
            Value = 50.00m,
            Date = DateTime.Now.AddDays(-1)
        };

        var command = new UpdateExpenseCommand("Updated Description", 100.00m, DateTime.Now)
        {
            Id = existingExpense.Id
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingExpense);

        _mockRepository
            .Setup(r => r.ExistsByDescriptionInTheSameMonthAsync(command.Description, command.Date))
            .ReturnsAsync(false);

        _mockRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Expense>()))
            .ReturnsAsync(true);

        var handler = new UpdateExpensesCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.UpdateAsync(It.Is<Expense>(e =>
            e.Id == command.Id &&
            e.Description == command.Description &&
            e.Value == command.Value &&
            e.Date == command.Date)), Times.Once);
    }

    [Fact]
    public async Task UpdateExpenseCommandHandler_ShouldReturnFalse_WhenExpenseNotFound()
    {
        // Arrange
        var command = new UpdateExpenseCommand("Updated Description", 100.00m, DateTime.Now)
        {
            Id = 999
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync((Expense)null);

        var handler = new UpdateExpensesCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task UpdateExpenseCommandHandler_ShouldThrowDuplicateDescriptionException_WhenDuplicateExists()
    {
        // Arrange
        var existingExpense = new Expense
        {
            Id = 1,
            Description = "Original Description",
            Value = 50.00m,
            Date = DateTime.Now.AddDays(-1)
        };

        var command = new UpdateExpenseCommand("Duplicate Description", 100.00m, DateTime.Now)
        {
            Id = existingExpense.Id
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(command.Id))
            .ReturnsAsync(existingExpense);

        _mockRepository
            .Setup(r => r.ExistsByDescriptionInTheSameMonthAsync(command.Description, command.Date))
            .ReturnsAsync(true);

        var handler = new UpdateExpensesCommandHandler(_mockRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DuplicateDescriptionException>(() =>
            handler.Handle(command, CancellationToken.None));

        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Expense>()), Times.Never);
    }

    [Fact]
    public async Task DeleteExpenseCommandHandler_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        const int expenseId = 1;
        var command = new DeleteExpenseCommand(expenseId);

        _mockRepository
            .Setup(r => r.DeleteAsync(expenseId))
            .ReturnsAsync(true);

        var handler = new DeleteExpensesCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.DeleteAsync(expenseId), Times.Once);
    }

    [Fact]
    public async Task DeleteExpenseCommandHandler_ShouldReturnFalse_WhenExpenseNotFound()
    {
        // Arrange
        const int expenseId = 999;
        var command = new DeleteExpenseCommand(expenseId);

        _mockRepository
            .Setup(r => r.DeleteAsync(expenseId))
            .ReturnsAsync(false);

        var handler = new DeleteExpensesCommandHandler(_mockRepository.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.DeleteAsync(expenseId), Times.Once);
    }
}