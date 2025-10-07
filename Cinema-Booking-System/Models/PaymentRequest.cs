using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.Models
{
    public class PaymentRequest
    {
        [Required]
        [Display(Name = "Card Number")]
        [StringLength(19, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 19 digits")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Cardholder Name")]
        [StringLength(100, ErrorMessage = "Cardholder name cannot exceed 100 characters")]
        public string CardholderName { get; set; } = string.Empty;

        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        [Display(Name = "Expiry Month")]
        public int ExpiryMonth { get; set; }

        [Required]
        [Range(2024, 2050, ErrorMessage = "Year must be between 2024 and 2050")]
        [Display(Name = "Expiry Year")]
        public int ExpiryYear { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 digits")]
        [Display(Name = "CVV")]
        public string CVV { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Currency must be 3 characters")]
        public string Currency { get; set; } = "USD";

        [Display(Name = "Billing Address")]
        [StringLength(200, ErrorMessage = "Billing address cannot exceed 200 characters")]
        public string? BillingAddress { get; set; }

        [Display(Name = "City")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string? City { get; set; }

        [Display(Name = "Postal Code")]
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        public string? PostalCode { get; set; }

        [Display(Name = "Country")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string? Country { get; set; }
    }
}
