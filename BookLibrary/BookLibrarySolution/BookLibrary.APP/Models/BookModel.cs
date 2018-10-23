using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.APP.Models
{
    public class BookModel
    {
        public string Author { get; set; }

        public int AuthorId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishingDate { get; set; }

    }
}
