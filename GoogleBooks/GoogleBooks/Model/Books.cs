namespace GoogleBooks.Model
{
    public class Books
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public Item[] items { get; set; }
    }
}
