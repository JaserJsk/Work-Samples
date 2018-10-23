using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.API.Entities
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string AuthorName { get; set; }

        override
        public string ToString()
        {
            return AuthorName;
        }

        public ICollection<Book> Books { get; set; }

        // Initialize an empty list to avoid Null reference exception when trying to manipulate 
        // list when books have not loaded yet.
            = new List<Book>();
    }
}
