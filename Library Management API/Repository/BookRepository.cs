using Library_Management_API.Models;

namespace Library_Management_API.Repositories
{
    public static class BooksRepository
    {
        // Static list to hold books in memory
        public static List<Book> Books { get; set; } = new List<Book>();
    }
}

