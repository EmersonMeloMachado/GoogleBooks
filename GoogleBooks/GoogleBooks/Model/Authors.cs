using SQLite;

namespace GoogleBooks.Model
{
    public class Authors
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string name { get; set; }
    }
}
