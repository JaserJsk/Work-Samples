using System.Collections.Generic;

namespace BookLibrary.APP.Models
{
    public class BookPageModel
    {
        public string searchTerm { get; set; }

        public string searchType { get; set; }

        public IEnumerable<BookModel> BookModels { get; set; }
    }
}
