using System;
using System.Linq;
using Xamarin.Forms;
using Acr.UserDialogs;
using GoogleBooks.Data;
using GoogleBooks.Model;
using Xamarin.Essentials;
using GoogleBooks.Helpers;
using System.Windows.Input;
using System.Threading.Tasks;
using GoogleBooks.Model.Base;
using GoogleBooks.ViewModel.Base;
using System.Collections.Generic;
using GoogleBooks.Model.Navigation;
using GoogleBooks.Service.Contracts;
using System.Collections.ObjectModel;

namespace GoogleBooks.ViewModel
{
    public class BooksViewModel : BaseViewModel
    {
        #region Fields
        private Books books;
        private BaseBooks itemSelected;
        public ObservableCollection<BaseBooks> baseBooks;
        public ObservableCollection<BaseBooks> filteredBaseBooks;
        private string search;
        private bool isFavorites;
        private string selectedBooksId;
        private const int PageSize = 5;
        private int PageIndex = 0;
        private int PageCount = 0;

        private readonly IBooksService BooksService;
        #endregion Fields

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

        public ObservableCollection<BaseBooks> FilteredBaseBooks
        {
            get => filteredBaseBooks;
            set => SetProperty(ref filteredBaseBooks, value);
        }

        public bool IsFavorites
        {
            get => isFavorites;
            set => SetProperty(ref isFavorites, value);
        }

        public string Search
        {
            get => search;
            set
            {
                SetProperty(ref search, value);
                FilterAsync(search);
            }
        }

        public BooksViewModel(IBooksService BooksService, INavigationService navigationService, IUserDialogs userDialogs) : 
            base(navigationService, userDialogs)
        {
            Books = new Books();
            BaseBooks = new ObservableCollection<BaseBooks>();
            FilteredBaseBooks = new ObservableCollection<BaseBooks>();

            this.BooksService = BooksService;
        }

        public override async Task OnInitialize()
        {
            try
            {
                await OnLoadPageData().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand => _searchCommand ?? (_searchCommand = new Command<string>((text) =>
        {
            try
            {
                if (IsBusy)
                    return;

                if (text.Length > 3)
                    FilterAsync(text);


                IsBusy = false;
            }
            catch (Exception ex)
            {

                IsBusy = false;
                Console.WriteLine(ex.Message);
            }
        }));
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
        public ICommand FilterFavoritesCommand => new Command(async () =>
        {
            try
            {
                if (IsBusy)
                    return;

                IsFavorites = IsFavorites == false ? true : false;

               await FilterAsync();
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
                IsBusy = true;

                if(PageIndex <=  PageCount && IsFavorites == false)
                {
                    var baseBooks = await GetBookDtoAsync().ConfigureAwait(true);
                    if (baseBooks.Count > 0)
                    {
                        foreach (var item in baseBooks.Skip(PageIndex * PageSize).Take(PageSize).ToList())
                        {
                            item.Thumbnail = Convert.FromBase64String(item.Image);
                            BaseBooks.Add(item);
                        }
                    }

                    PageIndex += 1;
                }

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
            Device.BeginInvokeOnMainThread(async () =>
            {
                var baseBooks = await GetBooks(pageIndex: PageIndex, pageSize: PageSize).ConfigureAwait(true);
                foreach (var item in baseBooks)
                {
                    BaseBooks.Add(item);
                }

                PageIndex += 1;
            });
        }
        private async Task<List<BaseBooks>> GetBooks(int pageIndex, int pageSize)
        {
            var baseBooks = new ObservableCollection<BaseBooks>();
            try
            {
                IsBusy = true;

                FilteredBaseBooks.Clear();

                MockSkeleton();

                var BooksDto = await GetBookDtoAsync();
                if (BooksDto.Count == 0)
                {
                    baseBooks = await GetBookService();
                    foreach (var item in baseBooks)
                    {
                        item.Image = Convert.ToBase64String(item.Thumbnail);
                        await MobileDatabase.Current.Save(item);
                        FilteredBaseBooks.Add(item);
                    }

                    PageCount = baseBooks.Count / PageSize;
                }
                else
                {
                    foreach (var item in BooksDto)
                    {
                        BaseBooks b = new BaseBooks();
                        b.IdBooks = item?.IdBooks;
                        b.Title = item.Title;
                        b.Description = item.Description;
                        b.Authors = item.Authors;
                        b.BuyLink = item.BuyLink;
                        b.IsFavorite = item.IsFavorite;
                        b.Image = item.Image;
                        b.Thumbnail = Convert.FromBase64String(item.Image);
                        baseBooks.Add(b);
                        FilteredBaseBooks.Add(item);
                    }

                    PageCount = baseBooks.Count / PageSize;
                }

                BaseBooks.Clear();

                IsBusy = false;
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

            return baseBooks.Skip(pageIndex * pageSize).Take(pageSize).ToList();
        }

        private void MockSkeleton()
        {
            
            var baseBooks = new List<BaseBooks>();
            for (int i = 0; i < 10; i++)
            {
                baseBooks.Add(new BaseBooks
                {
                    Title = "",
                    Thumbnail = new byte[0]
                });
            }

            BaseBooks = new ObservableCollection<BaseBooks>(baseBooks);

        }

        private async Task<List<BaseBooks>> GetBookDtoAsync()
        {
            var BooksDto = await MobileDatabase.Current.Get<BaseBooks>();

            return BooksDto;
        }

        private async Task<ObservableCollection<BaseBooks>> GetBookService()
        {
            var baseBooks = new ObservableCollection<BaseBooks>();
            var internet = Connectivity.NetworkAccess;
            if(internet == NetworkAccess.Internet)
            {
                var books = await BooksService.GetBooks("mobiledevelopment").ConfigureAwait(true);
                if (books.items.Any())
                {
                    var itens = books.items.ToList();
                    for (int i = 0; i < itens.Count; i++)
                    {
                        BaseBooks b = new BaseBooks();
                        b.IdBooks = itens[i]?.id;
                        b.Title = itens[i]?.volumeInfo?.title;
                        b.Description = itens[i]?.volumeInfo?.description;
                        b.Authors = itens[i]?.volumeInfo?.authors.ToList();
                        b.BuyLink = itens[i]?.saleInfo?.buyLink;
                        b.Thumbnail = await BooksService.GetBooksImage(itens[i]?.volumeInfo?.imageLinks?.thumbnail).ConfigureAwait(true);
                        baseBooks.Add(b);
                    }
                    return baseBooks;
                }
            }

            return null;
        }

        public async Task FilterAsync(string text = null)
        {
            if (!string.IsNullOrEmpty(text))
            {
                BaseBooks = new ObservableCollection<BaseBooks>();
                var baseBooks = FilteredBaseBooks?.Where(x => x.Title.ToUpper().RemoveDiacritics().Contains(text.ToUpper().RemoveDiacritics())).ToList();
                if (baseBooks.Count > 0)
                {
                    if (IsFavorites == true)
                    {
                        foreach (var item in baseBooks)
                        {
                            if (item.IsFavorite == true)
                            {
                                item.Thumbnail = Convert.FromBase64String(item.Image);
                                BaseBooks.Add(item);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in baseBooks)
                        {
                            item.Thumbnail = Convert.FromBase64String(item.Image);
                            BaseBooks.Add(item);
                        }
                    }
                }
            }
            else
            {
                BaseBooks = new ObservableCollection<BaseBooks>();
                if (IsFavorites == true)
                {
                    foreach (var item in FilteredBaseBooks)
                    {
                        if(item.IsFavorite == true)
                        {
                            item.Thumbnail = Convert.FromBase64String(item.Image);
                            BaseBooks.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (var item in FilteredBaseBooks)
                    {
                        item.Thumbnail = Convert.FromBase64String(item.Image);
                        BaseBooks.Add(item);
                    }
                }
                
            }
        }
    }
}
