using DryIoc;
using System;
using Acr.UserDialogs;
using static DryIoc.Made;
using GoogleBooks.Service.Http;
using System.Collections.Concurrent;
using GoogleBooks.Service.Contracts;
using GoogleBooks.Service.Navigation;
using GoogleBooks.Service.Implementation;

namespace GoogleBooks.ViewModel.Base
{
    public class ViewModelLocator
    {
        public Container containerBuilder;

        internal ConcurrentDictionary<Type, Type> mappings;

        private static readonly Lazy<ViewModelLocator> lazyViewModel = new Lazy<ViewModelLocator>(() => new ViewModelLocator());

        public static ViewModelLocator Current => lazyViewModel.Value;

        public ViewModelLocator()
        {
            containerBuilder = new Container(rules => rules.WithoutFastExpressionCompiler());

            containerBuilder.Register<INavigationService, NavigationService>();
            containerBuilder.Register(made: Of(() => UserDialogs.Instance));
            containerBuilder.Register<IBooksService, BooksService>();
            containerBuilder.Register<IBrowser, Browser>();

            mappings = new ConcurrentDictionary<Type, Type>();
        }

        public T Resolve<T>() => containerBuilder.Resolve<T>();

        public object Resolve(Type type) => containerBuilder.Resolve(type);

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface =>
            containerBuilder.Register<TInterface, TImplementation>();

        public void Register<T>() where T : class => containerBuilder.Register<T>();

        public void RegisterForNavigation<TView, TViewModel>(IReuse reuseItem = null)
        where TViewModel : BaseViewModel
        {
            containerBuilder.Register<TViewModel>(reuse: reuseItem, ifAlreadyRegistered: IfAlreadyRegistered.Keep, setup: DryIoc.Setup.With(trackDisposableTransient: true));
            containerBuilder.Register<TView>(reuse: reuseItem, ifAlreadyRegistered: IfAlreadyRegistered.Keep, setup: DryIoc.Setup.With(trackDisposableTransient: true));
            AddMappings(typeof(TViewModel), typeof(TView));
        }

        public void Dispose()
        {
            containerBuilder.Dispose();
        }

        private void AddMappings(Type typeIn, Type typeOut)
        {
            mappings.AddOrUpdate(typeIn, typeOut, (key, existingVal) =>
            {
                if (typeOut != existingVal)
                {
                    throw new ArgumentException("Duplicate values are not allowed: {0}.", typeOut.Name);
                }
                return existingVal;
            });
        }
    }
}
