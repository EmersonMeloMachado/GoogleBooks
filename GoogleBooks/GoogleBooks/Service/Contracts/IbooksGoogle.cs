using GoogleBooks.Model;
using System.Threading.Tasks;

namespace GoogleBooks.Service.Contracts
{
    public interface IbooksGoogle
    {
        Task<Books> GetBooks(string books);
    }
}
