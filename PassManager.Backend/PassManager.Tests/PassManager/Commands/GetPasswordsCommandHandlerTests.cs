using PassManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;
using PassManager.WebAPI.Controllers;

namespace PassManager.Tests.Controllers
{
    public class PasswordControllerTests_Get : BaseControllerTests<PasswordController>
    {
        public PasswordControllerTests_Get() : base() { }

        protected override PasswordController CreateController() => new PasswordController(MockRepo.Object);

        [Fact]
        public async Task GetPasswords_ReturnsOkResult_WithAListOfPasswords()
        {
            // Arrange
            var passwords = new List<PasswordRecord>
            {
                new PasswordRecord { Id = 1, Name = "Test1", Password = "123", Type = "TestType", CreatedAt = DateTime.UtcNow },
                new PasswordRecord { Id = 2, Name = "Test2", Password = "456", Type = "TestType", CreatedAt = DateTime.UtcNow }
            };

            MockRepo.Setup(repo => repo.GetPasswordsAsync()).ReturnsAsync(passwords);

            // Act
            var result = await Controller.GetPasswords();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<PasswordRecord>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }
    }
}
