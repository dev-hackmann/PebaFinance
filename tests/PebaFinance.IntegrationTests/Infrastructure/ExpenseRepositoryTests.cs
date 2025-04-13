using PebaFinance.Domain.Models;
using PebaFinance.Infrastructure.Data.Repositories;
using Xunit;

namespace PebaFinance.IntegrationTests.Infrastructure;

public class ExpenseRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly ExpensesRepository _repository;

    public ExpenseRepositoryTests(DatabaseFixture fixture)
    {
        _repository = new ExpensesRepository(fixture);
    }

    [Fact]
    public async Task AddExpense_ShouldReturnNewId()
    {
        // Arrange
        var expense = new Expense { Description = "Test Expense", Value = 100, Date = DateTime.Now };

        // Act
        var id = await _repository.AddAsync(expense);

        // Assert
        Assert.True(id > 0);
    }
}