using GoogleBooks.ViewModel.Base;

namespace GoogleBooks.Model.Base
{
    public class BaseBooks : ModelBase
    {
        private string idBooks;
        private string title;
        private string description;
        public byte[] thumbnail;

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

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public byte[] Thumbnail
        {
            get => thumbnail;
            set => SetProperty(ref thumbnail, value);
        }
    }
}
