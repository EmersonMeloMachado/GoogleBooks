using GoogleBooks.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GoogleBooks.Service.Contracts
{
    public interface IBooksService
    {
        Task<Books> GetBooks(string books);

        Task<byte[]> GetBooksImage(string thumbnail);
    }
}
