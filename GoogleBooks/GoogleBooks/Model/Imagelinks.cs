using Xamarin.Forms;

namespace GoogleBooks.Model
{
    public class Imagelinks
    {
        public string smallThumbnail { get; set; }
        public string thumbnail { get; set; }
        public byte[] image { get; set; }
        public ImageSource Source { get; set; }
    }
}
