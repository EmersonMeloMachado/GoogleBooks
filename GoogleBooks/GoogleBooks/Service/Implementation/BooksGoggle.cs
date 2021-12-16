using GoogleBooks.Model;
using System.Threading.Tasks;
using GoogleBooks.Service.Http;
using GoogleBooks.Service.Contracts;
using System.Collections.ObjectModel;

namespace GoogleBooks.Service.Implementation 
{
    public class BooksGoggle : IBooksGoogle
    {
        private readonly IBooksGoogle booksGoogle;

        public BooksGoggle(IBooksGoogle booksGoogle)
        {
            this.booksGoogle = booksGoogle;
        }

        public Task<Books> GetBooks(string books) => HttpService.Current.Get<Books>(
                url: $"https://www.googleapis.com/books/v1/volumes?q={books}");
    }
}
