using GoogleBooks.Model;
using System.Threading.Tasks;
using GoogleBooks.Service.Http;
using GoogleBooks.Service.Contracts;
using System.Collections.ObjectModel;

namespace GoogleBooks.Service.Implementation 
{
    public class BooksGoggle : IBooksGoogle
    {
        public Task<Books> GetBooks(string books) => HttpService.Current.Get<Books>(
                url: $"https://www.googleapis.com/books/v1/volumes?q={books}");

        public Task<byte[]> GetBooksImage(string thumbnail) => HttpService.Current.GetImage<string>(
               url: thumbnail);
    }
}
