using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookLibrary.API.Entities
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Required]
        [MaxLength(400)]
        public string Description { get; set; }

        [Required]
        [DataType("decimal(18, 2")]
        public decimal Price { get; set; }

        [Column(TypeName = "date")]
        public DateTime PublishingDate { get; set; }

        [ForeignKey("AuthorId")]
        public Author Author { get; set; }

        public int AuthorId { get; set; }
    }
}
