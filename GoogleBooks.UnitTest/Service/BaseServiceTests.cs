using Moq;
using System;
using System.Threading.Tasks;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.UnitTest.Service
{
    public abstract class BaseServiceTests<TService> 
    {
        protected Mock<IBooksService> BooksService { get; }

        protected TService Service { get; }

        protected BaseServiceTests()
        {
            BooksService = new Mock<IBooksService>();

            BeforeCreateService();

            Service = BuildService();
        }

        protected virtual void BeforeCreateService()
        {
            BooksService.SetupAllProperties();
        }

        protected abstract TService BuildService();
    }
}
