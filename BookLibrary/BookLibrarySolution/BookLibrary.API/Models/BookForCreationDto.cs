using System;
using System.ComponentModel.DataAnnotations;

namespace BookLibrary.API.Models
{
    public class BookForCreationDto
    {
        [Required(ErrorMessage = "You should provide a title value.")]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required(ErrorMessage = "You should provide a genre value.")]
        [MaxLength(50)]
        public string Genre { get; set; }

        [Required(ErrorMessage = "You should provide a description value.")]
        [MaxLength(200)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime PublishingDate { get; set; }
        
    }
}
