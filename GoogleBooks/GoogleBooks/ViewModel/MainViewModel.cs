using GoogleBooks.Service.Contracts;
using GoogleBooks.ViewModel.Base;

namespace GoogleBooks.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
