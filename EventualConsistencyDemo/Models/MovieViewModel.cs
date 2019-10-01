using System.Collections.Generic;

namespace EventualConsistencyDemo.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; }
        public List<Theater> Theaters { get; set; }
    }
}
