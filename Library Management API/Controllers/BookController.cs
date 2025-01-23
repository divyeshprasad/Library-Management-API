using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Library_Management_API.Models;
using Library_Management_API.Repositories;

namespace LibraryManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/books
        [HttpGet]
        public List<Book> GetAllBooks()
        {
            return BooksRepository.Books;
        }

        // GET: api/books/{isbn}
        [HttpGet("{isbn}")]
        public ActionResult<Book> GetBookByIsbn(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
                return BadRequest("ISBN cannot be empty");

            Book book = BooksRepository.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (book == null)
                return NotFound($"The book with ISBN {isbn} was not found.");
            return Ok(book);
        }

        // GET: api/books/{title}
        [HttpGet("{title}")]
        public ActionResult<List<Book>> GetBooksByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return BadRequest("Title cannot be empty.");

            List<Book> books = BooksRepository.Books.Where(b => b.Title.Contains(title)).ToList();
            if (books.Count == 0)
                return NotFound($"No books found with the title containing {title}.");
            return Ok(books);
        }

        // POST: api/books
        [HttpPost]
        public ActionResult<Book> CreateBook(Book book)
        {
            if (BooksRepository.Books.Any(b => b.Isbn == book.Isbn))
            {
                return BadRequest("A book with this ISBN already exists.");
            }

            int newId = BooksRepository.Books.LastOrDefault()?.Id + 1 ?? 1; // Handle empty list
            book.Id = newId;
            BooksRepository.Books.Add(book);
            return CreatedAtAction(nameof(GetBookByIsbn), new { isbn = book.Isbn }, book);
        }

        // PUT: api/books/{isbn}
        [HttpPut("{isbn}")]
        public ActionResult<Book> UpdateBook(string isbn, Book updatedBook)
        {
            var existingBook = BooksRepository.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (existingBook == null)
                return NotFound($"The book with ISBN {isbn} was not found.");

            existingBook.Title = updatedBook.Title;
            existingBook.Author = updatedBook.Author;
            existingBook.Available = updatedBook.Available;

            return Ok(existingBook);
        }

        // DELETE: api/books/{isbn}
        [HttpDelete("{isbn}")]
        public ActionResult<bool> DeleteBookByIsbn(string isbn)
        {
            var book = BooksRepository.Books.FirstOrDefault(b => b.Isbn == isbn);
            if (book == null)
                return NotFound($"The book with ISBN {isbn} was not found.");

            BooksRepository.Books.Remove(book);
            return Ok(true);
        }
    }
}
