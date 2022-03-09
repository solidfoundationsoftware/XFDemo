using System;

namespace XFDemoApp.Models
{
    public class Listing
    {
        public string PMListingId { get; set; }
        public string ListingTitle { get; set; }
        public string Description { get; set; }
        public decimal ListingPrice { get; set; }
        public string ListingImageUrl { get; set; }
        public string OfferToLikersMessage { get; set; }
    }
}