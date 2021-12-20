using SQLite;
using GoogleBooks.ViewModel.Base;
using System.Collections.Generic;
using SQLiteNetExtensions.Attributes;
using GoogleBooks.Helpers;

namespace GoogleBooks.Model.Base
{
    public class BaseBooks : ModelBase
    {
        private string idBooks;
        private string title;
        private List<string> authors;
        private string description;
        private string buyLink;
        private string image;
        public byte[] thumbnail;
        private bool isFavorite;

        [PrimaryKey]
        public string IdBooks
        {
            get => idBooks;
            set => SetProperty(ref idBooks, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        [TextBlob("AuthorsBlobbed")]
        public List<string> Authors
        {
            get => authors;
            set => SetProperty(ref authors, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string BuyLink
        {
            get => buyLink;
            set => SetProperty(ref buyLink, value);
        }

        [TextBlob("ThumbnailBlobbed")]
        public byte[] Thumbnail
        {
            get => thumbnail;
            set => SetProperty(ref thumbnail, value);
        }
        public string Image
        {
            get => image;
            set => SetProperty(ref image, value);
        }

        public bool IsFavorite
        {
            get => isFavorite;
            set => SetProperty(ref isFavorite, value);
        }
    }
}
