using System;
using System.Collections.Generic;
using Shared.Entities;

namespace EventualConsistencyDemo.Areas.Admin.Models
{
    public class AdminReviewsModel
    {
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public IReadOnlyList<Review> Reviews { get; set; } = [];
    }
}
