using System;
using System.Threading.Tasks;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.Service.Implementation
{
    public class Browser : IBrowser
    {
        public async Task OpenAsync(Uri uri)
        {
            await Xamarin.Essentials.Browser.OpenAsync(uri).ConfigureAwait(false);
        }
    }
}
