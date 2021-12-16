using Xamarin.Forms;
using GoogleBooks.View;
using System.Threading;
using GoogleBooks.Helpers;
using GoogleBooks.ViewModel;
using System.Threading.Tasks;
using GoogleBooks.ViewModel.Base;
using GoogleBooks.Service.Contracts;

namespace GoogleBooks
{
    public partial class App : Application
    {
        private INavigationService navigationService;
        public App()
        {
            InitializeComponent();
            RegisterViewModels();
            StartSettings();
            InitializeApp();
        }

        private static void RegisterViewModels()
        {
            ViewModelLocator.Current.RegisterForNavigation<MainView, MainViewModel>();
        }

        private void InitializeApp()
        {
            ThreadHelper.Init(SynchronizationContext.Current);
            ThreadHelper.RunOnUIThread(async () => { await InitializeNavigation(); });
        }

        private void StartSettings()
        {
            navigationService = ViewModelLocator.Current.Resolve<INavigationService>();
        }

        private async Task InitializeNavigation()
        {
            await navigationService.InitializeAsync<MainViewModel>(null, true);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
