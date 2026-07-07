using System.Collections.Generic;
using Shared.Entities;

namespace EventualConsistencyDemo.Models
{
    public class MovieViewModel
    {
        public Movie Movie { get; set; } = null!;
        public List<Theater> Theaters { get; set; } = [];
    }
}
