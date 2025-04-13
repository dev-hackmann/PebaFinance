using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using PebaFinance.Application.Commands;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace PebaFinance.FunctionalTests.Api;

public class ExpenseControllerTests : IClassFixture<WebApplicationFactory<PebaFinance.Program>>
{
    private readonly HttpClient _client;

    public ExpenseControllerTests(WebApplicationFactory<PebaFinance.Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateExpense_ShouldReturnCreated()
    {
        // Arrange
        var uniqueDescription = $"Test Expense {Guid.NewGuid()}";
        var command = new CreateExpenseCommand(uniqueDescription, 100, DateTime.Now);

        // Act
        var response = await _client.PostAsJsonAsync("/api/expenses", command);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task GetExpense_ShouldReturnNotFound_WhenExpenseDoesNotExist()
    {
        // Act
        var response = await _client.GetAsync("/api/expenses/999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}