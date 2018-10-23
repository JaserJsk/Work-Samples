using System;
using System.Collections.Generic;

namespace BookLibrary.API.Models
{
    public class DataFromFile
    {
        public string Author { get; set; }

        public string Title { get; set; }

        public string Genre { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public DateTime Publish_Date { get; set; }

        public Dictionary<String, List<DataFromFile>> catalog { get; set; }
        
    }
}
