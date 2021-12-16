using Acr.UserDialogs;
using GoogleBooks.Model;
using GoogleBooks.Service.Contracts;
using GoogleBooks.ViewModel.Base;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoogleBooks.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private Command searchCommand;
        private Books books;
        private string search;
        private bool isLoading;

        private readonly IBooksGoogle booksGoogle;

        public Books Books
        {
            get => books;
            set => SetProperty(ref books, value);
        }

        public Command SearchCommand
        {
            get => searchCommand;
            set => SetProperty(ref searchCommand, value);
        }

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        public string Search
        {
            get => search;
            set => SetProperty(ref search, value);
        }

        public MainViewModel(IBooksGoogle booksGoogle, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            Books = new Books();

            this.booksGoogle = booksGoogle;

        }

        private Command _loadCommand;
        public Command LoadCommand => _loadCommand ?? (_loadCommand = new Command(async () => await LoadCommandExecute(), () => IsNotBusy));
        private async Task LoadCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                Books = await this.booksGoogle.GetBooks(string.Empty);
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
                LoadCommand.ChangeCanExecute();
            }
        }
    }
}
