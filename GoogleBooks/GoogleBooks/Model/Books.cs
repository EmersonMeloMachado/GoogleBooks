using System.Collections.Generic;

namespace GoogleBooks.Model
{
    public class Books
    {
        public string kind { get; set; }
        public int totalItems { get; set; }
        public List<Item> items { get; set; }
    }
}
