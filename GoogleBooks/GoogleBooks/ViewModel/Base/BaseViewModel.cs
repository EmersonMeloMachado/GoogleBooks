using System;
using Xamarin.Forms;
using Acr.UserDialogs;
using Xamarin.Essentials;
using System.Threading.Tasks;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.ViewModel.Base
{
    public abstract class BaseViewModel : ModelBase
    {
        protected INavigationService NavigationService { get; }
        protected IUserDialogs UserDialogService { get; }

        protected bool Inscrito { get; set; }
        protected bool Disposed { get; set; }

        private bool isBusy;
        private NavigationParameters parameters;
        public bool IsNotBusy { get => !isBusy; }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public NavigationParameters Parameters
        {
            get => parameters;
            set => SetProperty(ref parameters, value);
        }

        public BaseViewModel(INavigationService navigationService, IUserDialogs userDialogs)
        {
            Inscrito = false;
            Disposed = false;

            NavigationService = navigationService;
            UserDialogService = userDialogs;
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
            return Task.FromResult(view);
        }

        public virtual Task OnViewDisappearingAsync(VisualElement view) => Task.FromResult(view);



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
