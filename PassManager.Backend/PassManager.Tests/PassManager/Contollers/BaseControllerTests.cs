using Moq;
using PassManager.Application.Interfaces;
using PassManager.WebAPI.Controllers;

namespace PassManager.Tests.Controllers
{
    public abstract class BaseControllerTests<TController>
        where TController : class
    {
        protected readonly Mock<IPasswordRepository> MockRepo;
        protected readonly TController Controller;

        protected BaseControllerTests()
        {
            MockRepo = new Mock<IPasswordRepository>();
            Controller = CreateController();
        }

        protected abstract TController CreateController();
    }
}