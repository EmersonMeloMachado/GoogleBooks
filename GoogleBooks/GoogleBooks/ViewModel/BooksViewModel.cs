using Acr.UserDialogs;
using GoogleBooks.Model;
using GoogleBooks.Model.Base;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Service.Contracts;
using GoogleBooks.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GoogleBooks.ViewModel
{
    public class BooksViewModel : BaseViewModel
    {
        private Books books;
        private Command searchCommand;
        private BaseBooks itemSelected;
        public ObservableCollection<BaseBooks> baseBooks;
        private string search;
        private bool isLoading;
        private string selectedBooksId;
        private int remainingItems = 0;
        private int offSetInicial = 0;
        private int offSetFinal = 0;

        private readonly IBooksGoogle booksGoogle;

        public Books Books
        {
            get => books;
            set => SetProperty(ref books, value);
        }

        public string SelectedBooksId
        {
            get { return selectedBooksId; }
            set { SetProperty(ref selectedBooksId, value); }
        }

        public BaseBooks ItemSelected
        {
            get { return itemSelected; }
            set { SetProperty(ref itemSelected, value); }
        }

        public ObservableCollection<BaseBooks> BaseBooks
        {
            get => baseBooks;
            set => SetProperty(ref baseBooks, value);
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

        public int RemainingItems
        {
            get { return remainingItems; }
            set { SetProperty(ref remainingItems, value); }
        }

        public int OffSetInicial
        {
            get { return offSetInicial; }
            set { SetProperty(ref offSetInicial, value); }
        }

        public int OffSetFinal
        {
            get { return offSetFinal; }
            set { SetProperty(ref offSetFinal, value); }
        }

        public BooksViewModel(IBooksGoogle booksGoogle, INavigationService navigationService, IUserDialogs userDialogs) : base(navigationService, userDialogs)
        {
            Books = new Books();
            BaseBooks = new ObservableCollection<BaseBooks>();

            this.booksGoogle = booksGoogle;

            IsBusy = true;

            _ = OnLoadPageData();
        }

        //public override async Task OnInitialize()
        //{
        //    try
        //    {
                
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        public ICommand SelectedCommand => new Command<BaseBooks>(async (book) =>
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                SelectedBooksId = book.IdBooks;
                var parameters = new NavigationParameters(NavigationParameterHandle.HandleSelectedBook, book);
                await NavigationService.NavigateToAsync<BookDetailViewModel>(parameters);

                IsBusy = false;
            }
            catch (Exception ex)
            {

                IsBusy = false;
                Console.WriteLine(ex.Message);
            }
        });

        public ICommand LoadCommand => new Command(async () =>
        {
            try
            {
                if (IsBusy)
                    return;

                BaseBooks = await FiltrarPersonagens();
            }
            catch (Exception ex)
            {
                IsBusy = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        });

        public ICommand ThresholdReachedCommand => new Command(async () =>
        {
            if (IsBusy)
                return;

            try
            {
                if (OffSetFinal == 0) return;

                IsBusy = true;
                await OnLoadPageData();
                //var items = new List<Pokemon>();

                //OffSetInicial = OffSetFinal;

                //PokemonList = await _pokeApi.ObterListaPokemons(OffSetInicial, OffSetFinal);

                //if (PokemonList != null)
                //{
                //    foreach (var poke in PokemonList.results)
                //    {
                //        var pokemon = await _pokeApi.ObterPokemon(poke.url);
                //        if (pokemon != null)
                //            items.Add(pokemon);
                //    }

                //    Pokemons.AddRange(items);
                //    OffSetFinal += 20;
                //}
                IsBusy = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        });

        public override async Task OnLoadPageData()
        {
            IsBusy = true;
            MockSkeleton();
            BaseBooks = await FiltrarPersonagens();
            IsBusy = false;
        }
        private async Task<ObservableCollection<BaseBooks>> FiltrarPersonagens(string filtro = null)
        {
            var baseBooks = new ObservableCollection<BaseBooks>();
            try
            {
                var books = await this.booksGoogle.GetBooks("ios");
                if (books.items.Any())
                {
                    var itens = books.items.ToList();
                    for (int i = 0; i < itens.Count; i++)
                    {
                        BaseBooks b = new BaseBooks();
                        b.IdBooks = itens[i].id;
                        b.Thumbnail = await this.booksGoogle.GetBooksImage(itens[i].volumeInfo.imageLinks.thumbnail);
                        b.Title = itens[i].volumeInfo.title;
                        b.Description = itens[i].volumeInfo.description;
                        baseBooks.Add(b);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return baseBooks;
        }

        private void MockSkeleton()
        {
            this.BaseBooks = new ObservableCollection<BaseBooks>(new List<BaseBooks>
            {
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                   Title = "",
                    Thumbnail =  new byte[0]
                },
                new BaseBooks
                {
                    Title = "",
                    Thumbnail =  new byte[0]
                }
            });;
        }
    }
}
