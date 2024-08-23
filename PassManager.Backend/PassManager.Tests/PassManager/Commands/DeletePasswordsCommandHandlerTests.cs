using PassManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using PassManager.WebAPI.Controllers;

namespace PassManager.Tests.Controllers
{
    public class PasswordControllerTests_Delete : BaseControllerTests<PasswordController>
    {
        public PasswordControllerTests_Delete() : base() { }

        protected override PasswordController CreateController() => new PasswordController(MockRepo.Object);

        [Fact]
        public async Task DeletePassword_ReturnsNoContentResult_WhenPasswordIsDeleted()
        {
            // Arrange
            var passwordId = 1;
            var passwordRecord = new PasswordRecord { Id = passwordId, Name = "Test1", Password = "123", Type = "TestType", CreatedAt = DateTime.UtcNow };

            MockRepo.Setup(repo => repo.GetPasswordByIdAsync(passwordId)).ReturnsAsync(passwordRecord);
            MockRepo.Setup(repo => repo.DeletePasswordAsync(passwordId)).Returns(Task.CompletedTask);

            // Act
            var result = await Controller.DeletePassword(passwordId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeletePassword_ReturnsNotFoundResult_WhenPasswordDoesNotExist()
        {
            // Arrange
            var passwordId = 1;

            MockRepo.Setup(repo => repo.GetPasswordByIdAsync(passwordId)).ReturnsAsync((PasswordRecord)null);

            // Act
            var result = await Controller.DeletePassword(passwordId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
