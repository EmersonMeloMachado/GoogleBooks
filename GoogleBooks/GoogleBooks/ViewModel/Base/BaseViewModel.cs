using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Threading.Tasks;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.ViewModel.Base
{
    public abstract class BaseViewModel : ModelBase
    {
        protected INavigationService NavigationService { get; }

        protected bool Inscrito { get; set; }
        protected bool Disposed { get; set; }


        public static event EventHandler OnLogout;

        private Command backCommand;
        private Command logoutCommand;
        private bool isBusy;
        private bool isRefreshing;
        private bool isListaVisivel;
        private bool semInternet;
        private NavigationParameters parameters;
        private string title;
        private bool showMessage;
        private string message;
        private ImageSource iconImageSource;

        public Command BackCommand
        {
            get => backCommand;
            protected set => SetProperty(ref backCommand, value);
        }

        public Command LogoutCommand
        {
            get => logoutCommand;
            protected set => SetProperty(ref logoutCommand, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }

        public bool IsListaVisivel
        {
            get => isListaVisivel;
            set => SetProperty(ref isListaVisivel, value);
        }

        public NavigationParameters Parameters
        {
            get => parameters;
            set => SetProperty(ref parameters, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool ShowMessage
        {
            get => showMessage;
            set => SetProperty(ref showMessage, value);
        }

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public ImageSource IconImageSource
        {
            get => iconImageSource;
            set => SetProperty(ref iconImageSource, value);
        }

        public BaseViewModel(INavigationService navigationService)
        {
            Inscrito = false;
            Disposed = false;
            semInternet = false;
            IsListaVisivel = true;

            NavigationService = navigationService;  
        }

        public virtual Task SetParameters(NavigationParameters parameters)
        {
            Parameters = parameters;
            return Task.CompletedTask;
        }

        public virtual Task OnInitialize()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnLoadPageData()
        {
            return Task.CompletedTask;
        }

        public virtual Task OnViewAppearingAsync(VisualElement view)
        {
            ShowNoConnectionMessage();
            return Task.FromResult(view);
        }

        public virtual Task OnViewDisappearingAsync(VisualElement view) => Task.FromResult(view);


        protected virtual async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            ShowNoConnectionMessage();

            if (semInternet && HasConnection(e.NetworkAccess))
            {
                semInternet = false;
                await OnInitialize();
            }
            else
            {
                semInternet = true;
            }
        }

        private void ShowNoConnectionMessage()
        {
            Message = "No internet connection.";
            ShowMessage = !HasConnection();
        }

        protected Func<bool> CanExecute()
        {
            return new Func<bool>(() => !IsBusy);
        }

        protected bool HasConnection()
        {
            return HasConnection(Connectivity.NetworkAccess);
        }

        protected bool HasConnection(NetworkAccess networkAccess)
        {
            return networkAccess == NetworkAccess.Internet;
        }
    }
}
