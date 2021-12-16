using Xamarin.Forms;
using System.Threading.Tasks;

namespace GoogleBooks.ViewModel.Base
{
    public interface IHandleViewDisappearing
    {
        Task OnViewDisappearingAsync(VisualElement view);
    }
}
