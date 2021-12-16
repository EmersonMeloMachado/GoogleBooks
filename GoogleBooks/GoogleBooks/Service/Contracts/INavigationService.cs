using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using GoogleBooks.ViewModel.Base;
using GoogleBooks.Model.Navigation;

namespace GoogleBooks.Service.Contracts
{
    public interface INavigationService
    {
        Task InitializeAsync<TViewModel>(NavigationParameters parametros = null, bool paginaNavegacao = false, NavigationPage paginaNavegacaoCustomizada = null) where TViewModel : BaseViewModel;

        Task InsertBeforeNavigationAsync<TViewModel, TViewModelBefore>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
                                                                                                             where TViewModelBefore : BaseViewModel;
        Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;
        Task NavigateToAsync<TViewModel>(NavigationParameters parametros = null) where TViewModel : BaseViewModel;
        Task NavigateToAsync(Type viewModelType);
        Task NavigateToAsync(Type viewModelType, NavigationParameters parametros = null);
        Task NavigateAndClearBackStackAsync<TViewModel>(NavigationParameters parametros = null) where TViewModel : BaseViewModel;
    }
}
