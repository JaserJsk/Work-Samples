using AutoMapper;
using BookLibrary.API.Interfaces;
using BookLibrary.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BookLibrary.API.Controllers
{
    // Adding the route attribute to the controller class only once.
    [Route("api/authors")]
    public class BookController : Controller
    {
        // ILogger Instance.
        private ILogger<BookController> _logger;

        // IMailService Instance.
        private IMailService _mailService;

        // IBookLibraryRepository Instance.
        private IBookLibraryRepository _bookLibraryRepository;

        #region Constructor
        public BookController(ILogger<BookController> logger,
            IMailService mailService,
            IBookLibraryRepository bookStoreRepository)
        {
            // Adding the field Instances to the injected Instances.
            _logger = logger;
            _mailService = mailService;
            _bookLibraryRepository = bookStoreRepository;
        }
        #endregion

        /* --------------------------------- */

        #region GET [ Get All Books ]
        [HttpGet("books")]
        public IActionResult GetAllBooks()
        {
            try
            {
                var allBooks = _bookLibraryRepository.GetAllBooks();

                var allBooksResults = Mapper.Map<IEnumerable<BookWithAuthorDto>>(allBooks);

                return Ok(allBooksResults);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exeption while getting all books from all authors", ex);
                return StatusCode(500, "A problem happend while handeling your request.");
            }
        }
        #endregion

        #region GET [ Get Books By Id ]
        [HttpGet("{authorid}/books")]
        public IActionResult GetBooksById(int authorid)
        {
            try
            {
                if (!_bookLibraryRepository.AuthorExists(authorid))
                {
                    _logger.LogInformation($"Author with id {authorid} was not found when accessing books.");
                    return NotFound();
                }

                var bookForAuthor = _bookLibraryRepository.GetBookForAuthors(authorid);

                var bookForAuthorResults = Mapper.Map<IEnumerable<BookDto>>(bookForAuthor);

                return Ok(bookForAuthorResults);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exeption while getting book for author with id {authorid}.", ex);
                return StatusCode(500, "A problem happend while handeling your request.");
            }
        }
        #endregion

        #region GET [ Get All Books By Author ]
        [HttpGet("books/search/author/{author}")]
        public IActionResult GetAllBooksByAuthor(string author)
        {
            try
            {
                var allBooks = _bookLibraryRepository.GetAllBooksByAuthor(author);

                var allBooksResults = Mapper.Map<IEnumerable<BookWithAuthorDto>>(allBooks);

                return Ok(allBooksResults);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exeption while getting all books by author", ex);
                return StatusCode(500, "A problem happend while handeling your request.");
            }
        }
        #endregion

        #region GET [ Get All Books By Title ]
        [HttpGet("books/search/title/{title}")]
        public IActionResult GetAllBooksByTitle(string title)
        {
            try
            {
                var allBooks = _bookLibraryRepository.GetAllBooksByTitle(title);

                var allBooksResults = Mapper.Map<IEnumerable<BookWithAuthorDto>>(allBooks);

                return Ok(allBooksResults);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exeption while getting all books by title", ex);
                return StatusCode(500, "A problem happend while handeling your request.");
            }
        }
        #endregion

        #region GET [ Get All Books By Term ]
        [HttpGet("books/search/all/{term}")]
        public IActionResult GetAllBooksByTerm(string term)
        {
            try
            {
                var allBooks = _bookLibraryRepository.GetAllBooksByTerm(term);

                var allBooksResults = Mapper.Map<IEnumerable<BookWithAuthorDto>>(allBooks);

                return Ok(allBooksResults);
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exeption while getting all books by search term", ex);
                return StatusCode(500, "A problem happend while handeling your request.");
            }
        }
        #endregion

        #region GET [ GetBook Name = "Get Single Book" ]
        [HttpGet("{authorid}/books/{id}", Name = "Get Single Book")]
        public IActionResult GetSingleBook(int authorid, int id)
        {
            if (!_bookLibraryRepository.AuthorExists(authorid))
            {
                return NotFound();
            }

            var book = _bookLibraryRepository.GetBookForAuthor(authorid, id);
            if (book == null)
            {
                return NotFound();
            }

            var booktResult = Mapper.Map<BookDto>(book);

            return Ok(booktResult);
        }
        #endregion

        /* --------------------------------- */

        #region POST [ CreateBook ]
        [HttpPost("{authorid}/books")]
        public IActionResult CreateBook(int authorid,
            /* Request body will contain the data for the new book */
            [FromBody] BookForCreationDto book)
        {
            // If the data sent is corrupted or empty then it will return a Bad Request.
            if (book == null)
            {
                return BadRequest();
            }

            // This will generate an error if the Genre and Title are the same.
            if (book.Genre == book.Title)
            {
                ModelState.AddModelError("Genre", "The provided genre should be different from the title.");
            }

            // This will generate an error if the Description and Title are the same.
            if (book.Description == book.Title)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the title.");
            }

            // This will generate an error if the Description and Genre are the same.
            if (book.Description == book.Genre)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the genre.");
            }

            // ModelState is a dictionary that contains state of the model and model binding validations.
            // Will return false if an invalid value is sent.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var finalBook = Mapper.Map<Entities.Book>(book);

            _bookLibraryRepository.AddBookForAuthor(authorid, finalBook);

            if (!_bookLibraryRepository.Save())
            {
                return StatusCode(500, "A problem happend while handeling your request.");
            }

            var createdBookToReturn = Mapper.Map<Models.BookDto>(finalBook);

            return CreatedAtRoute("GetSingleBook", new
            { authorId = authorid, id = createdBookToReturn.Id }, createdBookToReturn);
        }
        #endregion

        /* --------------------------------- */

        #region PUT [ UpdateBook ]
        [HttpPut("{authorid}/books/{id}")]
        public IActionResult UpdateBook(int authorid, int id,
            /* Request body will contain the data for the updated book */
            [FromBody] BookForUpdateDto book)
        {
            // If the data sent is corrupted or empty then it will return a Bad Request.
            if (book == null)
            {
                return BadRequest();
            }

            // This will generate an error if the Genre and Title are the same.
            if (book.Genre == book.Title)
            {
                ModelState.AddModelError("Genre", "The provided genre should be different from the title.");
            }

            // This will generate an error if the Description and Title are the same.
            if (book.Description == book.Title)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the title.");
            }

            // This will generate an error if the Description and Genre are the same.
            if (book.Description == book.Genre)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the genre.");
            }

            // ModelState is a dictionary that contains state of the model and binding validations.
            // Will return false if an invalid value is sent.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookEntity = _bookLibraryRepository.GetBookForAuthor(authorid, id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(book, bookEntity);

            if (!_bookLibraryRepository.Save())
            {
                return StatusCode(500, "A problem happend while handeling your request.");
            }

            // Means that the request completed successfully but there is nothing to return.
            return NoContent();
        }
        #endregion

        #region PATCH [ PartiallyUpdateBook ]
        [HttpPatch("{authorid}/books/{id}")]
        public IActionResult PartiallyUpdateBook(int authorid, int id,
            /* Request body will contain the data for the updated book */
            [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {
            // If the data sent is corrupted or empty then it will return a Bad Request.
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_bookLibraryRepository.AuthorExists(authorid))
            {
                return NotFound();
            }

            var bookEntity = _bookLibraryRepository.GetBookForAuthor(authorid, id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            var bookToPatch = Mapper.Map<BookForUpdateDto>(bookEntity);

            // Passing in the object to patch.
            patchDoc.ApplyTo(bookToPatch, ModelState);

            // ModelState is a dictionary that contains state of the model and binding validations.
            // Will return false if an invalid value is sent.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // This will generate an error if the Genre and Title are the same.
            if (bookToPatch.Genre == bookToPatch.Title)
            {
                ModelState.AddModelError("Genre", "The provided genre should be different from the title.");
            }

            // This will generate an error if the Description and Title are the same.
            if (bookToPatch.Description == bookToPatch.Title)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the title.");
            }

            // This will generate an error if the Description and Genre are the same.
            if (bookToPatch.Description == bookToPatch.Genre)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the genre.");
            }

            // This will validate the BookForUpdate instance.
            TryValidateModel(bookToPatch);

            // ModelState is a dictionary that contains state of the model and binding validations.
            // Will return false if an invalid value is sent.
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(bookToPatch, bookEntity);

            if (!_bookLibraryRepository.Save())
            {
                return StatusCode(500, "A problem happend while handeling your request.");
            }

            // Means that the request completed successfully but there is nothing to return.
            return NoContent();
        }
        #endregion

        /* --------------------------------- */

        #region DELETE [ DeleteBook ]
        [HttpDelete("{authorid}/books/{id}")]
        public IActionResult DeleteBook(int authorid, int id)
        {
            // Before deleting book, must check if book exists.
            if (!_bookLibraryRepository.AuthorExists(authorid))
            {
                return NotFound();
            }

            var bookEntity = _bookLibraryRepository.GetBookForAuthor(authorid, id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            _bookLibraryRepository.DeleteBook(bookEntity);

            if (!_bookLibraryRepository.Save())
            {
                return StatusCode(500, "A problem happend while handeling your request.");
            }

            // Sending and email to developer notifying that a book has been deleted.
            _mailService.Send("Book deleted.",
                    $"Book {bookEntity.Title} with id {bookEntity.Id} was deleted.");

            // Means that the request completed successfully but there is nothing to return.
            return NoContent();
        }
        #endregion
    }
}
