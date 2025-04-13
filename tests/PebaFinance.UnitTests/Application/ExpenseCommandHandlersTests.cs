using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing; // Add this
using PebaFinance.Application.Commands;
using Xunit;

namespace PebaFinance.FunctionalTests.Api;

public class ExpenseControllerTests : IClassFixture<WebApplicationFactory<PebaFinance.>> // Update namespace
{
    private readonly HttpClient _client;

    public ExpenseControllerTests(WebApplicationFactory<PebaFinance.Program> factory) // Update namespace
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateExpense_ShouldReturnCreated()
    {
        // Arrange
        var command = new CreateExpenseCommand("Test Expense", 100, DateTime.Now);

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