using Xamarin.Forms;
using System.Threading.Tasks;

namespace GoogleBooks.ViewModel.Base
{
    public interface IHandleViewAppearing
    {
        Task OnViewAppearingAsync(VisualElement view);
    }
}
