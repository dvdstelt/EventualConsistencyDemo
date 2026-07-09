using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.Entities;

namespace EventualConsistencyDemo.Areas.Admin.Models
{
    public class MovieEditModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^[a-z0-9]+(-[a-z0-9]+)*$", ErrorMessage = "Use lower-case letters, digits and hyphens only, no spaces (e.g. star-wars-marathon).")]
        [Display(Name = "URL title")]
        public string UrlTitle { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Range(0, 5)]
        public int Rating { get; set; }

        [Display(Name = "Movie details")]
        public string MovieDetails { get; set; } = string.Empty;

        [Display(Name = "Trailer URL")]
        public string? Trailer { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release date")]
        public DateTime ReleaseDate { get; set; } = DateTime.Today;

        [Display(Name = "Price per ticket")]
        public double PricePerTicket { get; set; }

        [Display(Name = "Ticket type")]
        public TicketType TicketType { get; set; }

        [Display(Name = "Icons (comma separated)")]
        public string Icons { get; set; } = string.Empty;

        [Display(Name = "Showtimes (comma separated)")]
        public string Showtimes { get; set; } = string.Empty;

        // The poster cropped in the browser, sent as a data URL (image/jpeg, base64).
        public string? PosterData { get; set; }

        [Display(Name = "Background image")]
        public IFormFile? BackgroundFile { get; set; }

        // Current file names, shown on the edit screen.
        public string? ExistingImage { get; set; }
        public string? ExistingBackground { get; set; }
    }
}
