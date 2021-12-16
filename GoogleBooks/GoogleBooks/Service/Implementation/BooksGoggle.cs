using GoogleBooks.Model;
using System.Threading.Tasks;
using GoogleBooks.Service.Http;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.Service.Implementation 
{
    public class BooksGoggle : IbooksGoogle
    {
        public Task<Books> GetBooks(string books) => HttpService.Current.Get<Books>(
                url: $"https://www.googleapis.com/books/v1/volumes?q={books}");
    }
}
