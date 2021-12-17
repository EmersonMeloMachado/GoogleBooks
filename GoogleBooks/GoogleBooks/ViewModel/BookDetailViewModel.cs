using Acr.UserDialogs;
using GoogleBooks.ViewModel.Base;
using GoogleBooks.Service.Contracts;
using System.Threading.Tasks;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Model.Base;
using System;

namespace GoogleBooks.ViewModel
{
    public class BookDetailViewModel : BaseViewModel
    {
        private BaseBooks baseBooks;

        public BaseBooks BaseBooks
        {
            get => baseBooks;
            set => SetProperty(ref baseBooks, value);
        }

        public BookDetailViewModel(INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
        }

        public override async Task SetParameters(NavigationParameters parameters)
        {
            try
            {
                await base.SetParameters(parameters);
                BaseBooks = Parameters?.GetValue<BaseBooks>(NavigationParameterHandle.HandleSelectedBook);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
