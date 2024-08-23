using Moq;
using PassManager.Application.Interfaces;
using PassManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using PassManager.WebAPI.Controllers;

namespace PassManager.Tests
{
    public class PasswordManagerControllerTests
    {
        private readonly Mock<IPasswordRepository> _mockRepo;
        private readonly PasswordController _controller;

        public PasswordManagerControllerTests()
        {
            _mockRepo = new Mock<IPasswordRepository>();
            _controller = new PasswordController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetPasswords_ReturnsOkResult_WithAListOfPasswords()
        {
            // Arrange
            var passwords = new List<PasswordRecord>
    {
        new PasswordRecord { Id = 1, Name = "Test1", Password = "123", Type = "TestType", CreatedAt = DateTime.UtcNow },
        new PasswordRecord { Id = 2, Name = "Test2", Password = "456", Type = "TestType", CreatedAt = DateTime.UtcNow }
    };

            _mockRepo.Setup(repo => repo.GetPasswordsAsync()).ReturnsAsync(passwords);

            // Act
            var result = await _controller.GetPasswords();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);  // Исправление здесь
            var returnValue = Assert.IsType<List<PasswordRecord>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task AddPassword_ReturnsCreatedAtActionResult_WhenPasswordIsAdded()
        {
            // Arrange
            var passwordRecord = new PasswordRecord { Id = 1, Name = "Test1", Password = "123", Type = "TestType", CreatedAt = DateTime.UtcNow };

            _mockRepo.Setup(repo => repo.PasswordExistsAsync(passwordRecord.Name)).ReturnsAsync(false);
            _mockRepo.Setup(repo => repo.AddPasswordAsync(passwordRecord)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddPassword(passwordRecord);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);  // Исправление здесь
            var returnValue = Assert.IsType<PasswordRecord>(createdAtActionResult.Value);
            Assert.Equal(passwordRecord.Name, returnValue.Name);
        }


        [Fact]
        public async Task DeletePassword_ReturnsNoContentResult_WhenPasswordIsDeleted()
        {
            // Arrange
            var passwordId = 1;
            var passwordRecord = new PasswordRecord { Id = passwordId, Name = "Test1", Password = "123", Type = "TestType", CreatedAt = DateTime.UtcNow };

            _mockRepo.Setup(repo => repo.GetPasswordByIdAsync(passwordId)).ReturnsAsync(passwordRecord);
            _mockRepo.Setup(repo => repo.DeletePasswordAsync(passwordId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeletePassword(passwordId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePassword_ReturnsNotFoundResult_WhenPasswordDoesNotExist()
        {
            // Arrange
            var passwordId = 1;

            _mockRepo.Setup(repo => repo.GetPasswordByIdAsync(passwordId)).ReturnsAsync((PasswordRecord)null);

            // Act
            var result = await _controller.DeletePassword(passwordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
