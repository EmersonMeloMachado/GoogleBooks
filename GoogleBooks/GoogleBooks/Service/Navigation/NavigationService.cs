using System;
using System.Linq;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using GoogleBooks.ViewModel.Base;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks.Service.Navigation
{
    public class NavigationService : INavigationService
    {
        protected Application CurrentApplication
        {
            get { return Application.Current; }
        }

        public async Task InitializeAsync<TViewModel>(NavigationParameters parameters = null, bool navigationPage = false, NavigationPage customNavigationPage = null) where TViewModel : BaseViewModel
        {
            await InternalInitializeAsync(typeof(TViewModel), parameters, navigationPage, customNavigationPage);
        }

        public async Task InsertBeforeNavigationAsync<TViewModel, TViewModelBefore>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
                                                                                                                     where TViewModelBefore : BaseViewModel
        {
            await InternalInsertBeforeNavigationAsync(typeof(TViewModel), typeof(TViewModelBefore), parameters);
        }

        public async Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public async Task NavigateToAsync<TViewModel>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
        {
            await InternalNavigateToAsync(typeof(TViewModel), parameters);
        }

        public async Task NavigateToAsync(Type viewModelType)
        {
            await InternalNavigateToAsync(viewModelType, null);
        }

        public async Task NavigateToAsync(Type viewModelType, NavigationParameters parameters)
        {
            await InternalNavigateToAsync(viewModelType, parameters);
        }

        public async Task NavigateAndClearBackStackAsync<TViewModel>(NavigationParameters parameters = null) where TViewModel : BaseViewModel
        {
            try
            {
                Page page = await CreateAndBindPage(typeof(TViewModel), parameters);
                var navigationPage = CurrentApplication.MainPage as NavigationPage;

                await navigationPage.PushAsync(page);
                await ClearBackStackAsync(page, navigationPage);
            }
            catch (Exception ex)
            {
                throw new Exception($"NavigateAndClearBackStackAsync: {ex.Message}");
            }
        }

        private async Task ClearBackStackAsync(Page page, NavigationPage navigationPage)
        {
            var tasks = new List<Task>();
            if (navigationPage != null && navigationPage.Navigation.NavigationStack.Count > 0)
            {
                var existingPages = navigationPage.Navigation.NavigationStack.Where(existingPage => existingPage != page).ToList();
                foreach (var existingPage in existingPages)
                {
                    tasks.Add(RemovePage(existingPage, navigationPage));
                }
                await Task.WhenAll(tasks);
            }
        }

        private Task RemovePage(Page page, NavigationPage navigationPage)
        {
            navigationPage.Navigation.RemovePage(page);
            return Task.CompletedTask;
        }

        private async Task InternalNavigateToAsync(Type viewModelType, NavigationParameters parameters, bool modal = false)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);

                if (CurrentApplication.MainPage is NavigationPage currentNavigationPage)
                {
                    if (modal)
                    {
                        await CurrentApplication.MainPage.Navigation.PushModalAsync(page);
                    }
                    else
                    {
                        await currentNavigationPage.PushAsync(page);
                    }
                }
                else
                {
                    CurrentApplication.MainPage = new NavigationPage(page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task InternalInsertBeforeNavigationAsync(Type viewModelType, Type viewModelTypeBefore, NavigationParameters parameters)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);
                Page pageBefore = await CreateAndBindPage(viewModelTypeBefore, parameters);

                if (CurrentApplication.MainPage is NavigationPage currentNavigationPage)
                {
                    if (currentNavigationPage.Navigation.NavigationStack.All(p => p != page))
                    {
                        currentNavigationPage.Navigation.InsertPageBefore(page, pageBefore);
                        await currentNavigationPage.Navigation.PopAsync();
                    }
                }
                else
                {
                    CurrentApplication.MainPage = new NavigationPage(page);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task InternalInitializeAsync(Type viewModelType, NavigationParameters parameters, bool navigationPage = false, NavigationPage customNavigationPage = null)
        {
            try
            {
                Page page = await CreateAndBindPage(viewModelType, parameters);

                if (navigationPage)
                {
                    if (customNavigationPage != null)
                    {
                        CurrentApplication.MainPage = (NavigationPage)Activator.CreateInstance(customNavigationPage.GetType(), page);
                    }
                    else
                    {
                        CurrentApplication.MainPage = new NavigationPage(page);
                    }
                }
                else
                {
                    CurrentApplication.MainPage = page;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!ViewModelLocator.Current.mappings.ContainsKey(viewModelType))
            {
                KeyNotFoundException ex = new KeyNotFoundException($"No map for ${viewModelType} was found on navigation mappings");
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return ViewModelLocator.Current.mappings[viewModelType];
        }

        private async Task<Page> CreateAndBindPage(Type viewModelType, NavigationParameters parameters)
        {
            try
            {
                Type pageType = GetPageTypeForViewModel(viewModelType);

                if (pageType == null)
                {
                    Exception ex = new Exception($"Mapping type for {viewModelType} is not a page");
                    Console.WriteLine(ex.Message);
                    throw ex;
                }

                Page page = null;

                page = ViewModelLocator.Current.Resolve(pageType) as Page;
                BaseViewModel viewModel = ViewModelLocator.Current.Resolve(viewModelType) as BaseViewModel;
                await viewModel.SetParameters(parameters);
                await viewModel.OnInitialize();
                page.BindingContext = viewModel;

                return page;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
