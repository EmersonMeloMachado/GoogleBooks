using Xamarin.Forms;
using GoogleBooks.ViewModel.Base;

namespace GoogleBooks.View.Base
{
    public abstract class BasePage : ContentPage
    {
        private BaseViewModel ViewModel => BindingContext as BaseViewModel;


        public BasePage()
        {

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (ViewModel == null)
            {
                return;
            }

            if (ViewModel is IHandleViewAppearing viewAware)
            {
                await viewAware.OnViewAppearingAsync(this);
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            if (ViewModel is IHandleViewDisappearing viewAware)
            {
                await viewAware.OnViewDisappearingAsync(this);
            }
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return false;
        }
    }
}
