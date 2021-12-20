using GoogleBooks.Model;
using System.Threading.Tasks;
using GoogleBooks.Service.Http;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.Service.Implementation 
{
    public class BooksService : IBooksService
    {
        public Task<Books> GetBooks(string books) => HttpService.Current.Get<Books>(
                url: $"https://www.googleapis.com/books/v1/volumes?q={books}&maxResults=40&startIndex=0");

        public Task<byte[]> GetBooksImage(string thumbnail) => HttpService.Current.GetImage<string>(
               url: thumbnail);
    }
}
