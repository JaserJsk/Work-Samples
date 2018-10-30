using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.API.Models
{
    public class AuthorDto
    {
        public int Id { get; set; }

        public string AuthorName { get; set; }

        public int NumberOfBooks
        {
            get
            {
                // Calculate number of books for author.
                return Books.Count;
            }
        }

        // Extending the AuthorDto class so that it will contain a collection of books.
        // Initializing to an empty collection instead of leaving it as null, to avoid Null reference issues.
        // This can be done with C# 6.0 auto property initializer syntax.
        public ICollection<BookDto> Books { get; set; }
            = new List<BookDto>();
    }
}
