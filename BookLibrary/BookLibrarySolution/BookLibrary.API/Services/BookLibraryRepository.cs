using BookLibrary.API.Entities;
using BookLibrary.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.API.Services
{
    public class BookLibraryRepository : IBookLibraryRepository
    {
        private ApplicationContext _context;

        #region Constructor
        public BookLibraryRepository(ApplicationContext context)
        {
            _context = context;
        }
        #endregion

        /* --------------------------------- */

        #region Author Exists
        public bool AuthorExists(int authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        } 
        #endregion

        #region Get All Authors
        public IEnumerable<Author> GetAllAuthors()
        {
            return _context.Authors.OrderBy(a => a.AuthorName).ToList();
        } 
        #endregion

        #region Get Author
        public Author GetAuthor(int authorId, bool inludeBook)
        {
            if (inludeBook)
            {
                return _context.Authors.Include(a => a.Books)
                    .Where(a => a.Id == authorId).FirstOrDefault();
            }

            return _context.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }
        #endregion

        /* --------------------------------- */

        #region Get All Books
        public IEnumerable<Book> GetAllBooks()
        {
            var authors = this.GetAllAuthors();
            List<Book> allBooks = new List<Book>();
            foreach (var author in authors)
            {
                var authBooks = this.GetBookForAuthors(author.Id);
                foreach (var oneBook in authBooks)
                {
                    oneBook.Author = author;
                    allBooks.Add(oneBook);
                }
            }
            return allBooks;
        } 
        #endregion

        #region Get All Books By Author
        public IEnumerable<Book> GetAllBooksByAuthor(string authorname)
        {
            var authors = this.GetAllAuthors();
            List<Book> allBooks = new List<Book>();
            foreach (var author in authors)
            {
                var authBooks = this.GetBookForAuthors(author.Id);
                foreach (var oneBook in authBooks)
                {
                    oneBook.Author = author;
                    if (oneBook.Author.AuthorName.ToLower().Contains(authorname.ToLower()) ||
                        oneBook.Author.AuthorName.Contains(authorname))
                    {
                        allBooks.Add(oneBook);
                    }
                }
            }
            return allBooks;
        } 
        #endregion

        #region Get All Books By Title
        public IEnumerable<Book> GetAllBooksByTitle(string title)
        {
            var authors = this.GetAllAuthors();
            List<Book> allBooks = new List<Book>();
            foreach (var author in authors)
            {
                var authBooks = this.GetBookForAuthors(author.Id);
                foreach (var oneBook in authBooks)
                {
                    oneBook.Author = author;
                    if (oneBook.Title.ToLower().Contains(title.ToLower()) ||
                        oneBook.Title.Contains(title))
                    {
                        allBooks.Add(oneBook);
                    }
                }
            }
            return allBooks;
        } 
        #endregion

        #region Get All Books By Term
        public IEnumerable<Book> GetAllBooksByTerm(string term)
        {
            var authors = this.GetAllAuthors();
            List<Book> allBooks = new List<Book>();
            foreach (var author in authors)
            {
                var authBooks = this.GetBookForAuthors(author.Id);
                foreach (var oneBook in authBooks)
                {
                    oneBook.Author = author;
                    if (oneBook.Author.AuthorName.ToLower().Contains(term.ToLower()) ||
                        oneBook.Title.ToLower().Contains(term.ToLower()) ||
                        oneBook.Author.AuthorName.Contains(term) ||
                        oneBook.Title.Contains(term))
                    {
                        allBooks.Add(oneBook);
                    }
                }
            }
            return allBooks;
        }
        #endregion

        /* --------------------------------- */

        #region Get Book For Authors
        public IEnumerable<Book> GetBookForAuthors(int authorId)
        {
            return _context.Books
                .Where(b => b.AuthorId == authorId).ToList();
        }
        #endregion

        #region Get Book For Author
        public Book GetBookForAuthor(int authorId, int bookId)
        {
            return _context.Books
                .Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }
        #endregion

        /* --------------------------------- */

        #region Add Book For Author
        public void AddBookForAuthor(int authorId, Book book)
        {
            var author = GetAuthor(authorId, false);
            author.Books.Add(book);
        }
        #endregion

        /* --------------------------------- */

        #region Delete Book
        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }
        #endregion

        #region Save
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        } 
        #endregion
    }
}
