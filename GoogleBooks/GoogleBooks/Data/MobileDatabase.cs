using SQLite;
using System;
using System.IO;
using GoogleBooks.Model.Base;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GoogleBooks.Data
{
    sealed class MobileDatabase
    {
        private static Lazy<MobileDatabase> _Lazy = new Lazy<MobileDatabase>(() => new MobileDatabase());

        public static MobileDatabase Current { get => _Lazy.Value; }

        private MobileDatabase()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GoogleBooks.db3");
            _SQLiteAsyncConnection = new SQLiteAsyncConnection(path);
            var connection = new SQLiteConnection(path);
            connection.CreateTable<BaseBooks>();
        }

        private readonly SQLiteAsyncConnection _SQLiteAsyncConnection;

        public Task<List<T>> Get<T>() where T : new()
            => _SQLiteAsyncConnection.Table<T>().ToListAsync();

        public Task<T> Get<T>(object key) where T : new()
            => _SQLiteAsyncConnection.GetAsync<T>(pk: key);

        public async Task<int> Save<T>(T item)
        {
            var affected = await _SQLiteAsyncConnection.UpdateAsync(item);

            if (affected <= 0)
                affected = await _SQLiteAsyncConnection.InsertAsync(item);

            return affected;
        }

        public Task<int> Save<T>(IEnumerable<T> itens, bool runInTransaction = true)
            => _SQLiteAsyncConnection.UpdateAllAsync(itens, runInTransaction: runInTransaction);

        internal async Task DropTable<T>(bool recreate = true) where T : new()
        {
            await _SQLiteAsyncConnection.DropTableAsync<T>();

            if (recreate)
                await _SQLiteAsyncConnection.CreateTableAsync<T>();
        }

        internal async Task RemoveFavoriteBookId(string uID)
        {
            var Books = await _SQLiteAsyncConnection.Table<BaseBooks>().Where(lbda => !lbda.IdBooks.Equals(uID)).ToListAsync();
            if(Books.Count > 0)
            {
                foreach (var item in Books)
                {
                    item.IsFavorite = false;
                }
            }

            var affected = await _SQLiteAsyncConnection.UpdateAsync(Books);
        }

        internal async Task RemoveAllFavoriteBook()
        {
            var Books = await _SQLiteAsyncConnection.Table<BaseBooks>().ToListAsync();
            await _SQLiteAsyncConnection.DeleteAllAsync<BaseBooks>();
            await _SQLiteAsyncConnection.InsertAllAsync(Books);
        }
    }
}
