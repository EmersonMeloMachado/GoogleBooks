using GoogleBooks.Model;
using System.Threading.Tasks;

namespace GoogleBooks.Service.Implementation
{
    public interface IbooksGoogle
    {
        Task<Books> GetBooks(string books);
    }
}
