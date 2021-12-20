using Xunit;
using System.Threading.Tasks;
using GoogleBooks.Service.Implementation;

namespace GoogleBooks.UnitTest.Service
{
    public class BooksServiceTest : BaseServiceTests<BooksService>
    {
        protected override BooksService BuildService()
        {
            return new BooksService();
        }

        [Fact(DisplayName = "When I open the application for the first time, it must look for the data on the internet")]
        public async Task GetBooksAsync()
        {
            var serviceResponse = await Service.GetBooks("mobiledevelopment").ConfigureAwait(true);
            Assert.True(serviceResponse.totalItems > 0);
        }
    }
}
