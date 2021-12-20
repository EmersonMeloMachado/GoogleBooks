using System;
using Xamarin.Forms;
using Acr.UserDialogs;
using GoogleBooks.Model;
using System.Windows.Input;
using System.Threading.Tasks;
using GoogleBooks.Model.Base;
using GoogleBooks.ViewModel.Base;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Service.Contracts;
using System.Collections.ObjectModel;

namespace GoogleBooks.ViewModel
{
    public class BookDetailViewModel : BaseViewModel
    {
        private BaseBooks baseBooks;
        public ObservableCollection<Authors> authors;
        private readonly IBrowser browser;


        public ObservableCollection<Authors> Authors
        {
            get => authors;
            set => SetProperty(ref authors, value);
        }

        public BaseBooks BaseBooks
        {
            get => baseBooks;
            set => SetProperty(ref baseBooks, value);
        }

        public BookDetailViewModel(IBrowser browser,INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            this.browser = browser;
            Authors = new ObservableCollection<Authors>();
        }

        public override async Task SetParameters(NavigationParameters parameters)
        {
            try
            {
                await base.SetParameters(parameters);
                BaseBooks = Parameters?.GetValue<BaseBooks>(NavigationParameterHandle.HandleSelectedBook);
                if(BaseBooks.Authors.Count > 0)
                {
                    foreach (var item in BaseBooks.Authors)
                    {
                        Authors authors = new Authors();
                        authors.name = item;
                        Authors.Add(authors);
                    }
                }
                IsVisible = baseBooks.BuyLink == null ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public ICommand OpenBrowserCommand => new Command(async (book) =>
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                await browser.OpenAsync(new Uri(BaseBooks.BuyLink)).ConfigureAwait(false);

                IsBusy = false;
            }
            catch (Exception ex)
            {

                IsBusy = false;
                Console.WriteLine(ex.Message);
            }
        });
    }
}
