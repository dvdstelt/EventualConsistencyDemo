using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace EventualConsistencyDemo.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string UrlTitle { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }

        public int Rating { get; set; }
        public string Description { get; set; }
        public List<string> Icons { get; set; }
    }
}
