using GoogleBooks.Model;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GoogleBooks.Service.Contracts
{
    public interface IBooksGoogle
    {
        Task<Books> GetBooks(string books);
    }
}
