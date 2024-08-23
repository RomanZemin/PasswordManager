using PassManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using PassManager.WebAPI.Controllers;

namespace PassManager.Tests.Controllers
{
    public class PasswordControllerTests_Add : BaseControllerTests<PasswordController>
    {
        public PasswordControllerTests_Add() : base() { }

        protected override PasswordController CreateController() => new PasswordController(MockRepo.Object);

        [Fact]
        public async Task AddPassword_ReturnsCreatedAtActionResult_WhenPasswordIsAdded()
        {
            // Arrange
            var passwordRecord = new PasswordRecord { Id = 1, Name = "Test1", Password = "123", Type = "TestType", CreatedAt = DateTime.UtcNow };

            MockRepo.Setup(repo => repo.PasswordExistsAsync(passwordRecord.Name)).ReturnsAsync(false);
            MockRepo.Setup(repo => repo.AddPasswordAsync(passwordRecord)).Returns(Task.CompletedTask);

            // Act
            var result = await Controller.AddPassword(passwordRecord);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<PasswordRecord>(createdAtActionResult.Value);
            Assert.Equal(passwordRecord.Name, returnValue.Name);
        }
    }
}
