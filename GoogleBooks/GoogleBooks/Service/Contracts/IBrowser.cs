using System;
using System.Threading.Tasks;

namespace GoogleBooks.Service.Contracts
{
    public interface IBrowser
    {
        Task OpenAsync(Uri uri);
    }
}
