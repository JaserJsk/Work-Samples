using AutoMapper;
using BookLibrary.API.Interfaces;
using BookLibrary.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookLibrary.API.Controllers
{
    // Adding the route attribute to the controller class only once.
    [Route("api/_authors")]
    public class AuthorController : Controller
    {
        private IBookLibraryRepository _bookLibraryRepository;

        #region Constructor
        public AuthorController(IBookLibraryRepository bookStoreRepository)
        {
            _bookLibraryRepository = bookStoreRepository;
        }
        #endregion

        /* --------------------------------- */

        #region GET [ GetAuthors ]
        [HttpGet()]
        public IActionResult GetAuthors()
        {
            var authorEntities = _bookLibraryRepository.GetAllAuthors();
            var results = Mapper.Map<IEnumerable<AuthorWithoutBookDto>>(authorEntities);

            return Ok(results);
        }
        #endregion

        #region GET [ GetAuthor ]
        [HttpGet("{authorid}")]
        public IActionResult GetAuthor(int authorid, bool includeBook = false)
        {
            var author = _bookLibraryRepository.GetAuthor(authorid, includeBook);

            if (author == null)
            {
                return NotFound();
            }

            if (includeBook)
            {
                var authorResult = Mapper.Map<IEnumerable<AuthorDto>>(author);

                return Ok(authorResult);
            }

            var authorWithoutBookResult = Mapper.Map<AuthorWithoutBookDto>(author);

            return Ok(authorWithoutBookResult);
        }
        #endregion
    }
}
