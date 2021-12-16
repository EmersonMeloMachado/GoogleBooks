using Xamarin.Forms;

namespace GoogleBooks.View.Base
{
    public interface IPage
    {
        string Title { get; set; }

        string AutomationId { get; set; }

        ImageSource IconImageSource { get; set; }
    }
}
