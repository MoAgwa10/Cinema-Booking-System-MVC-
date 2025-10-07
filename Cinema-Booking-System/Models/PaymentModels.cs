using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cinema_Booking_System.Models
{
    // Note: PaymentRequest, PaymentResult, and CardType are now in separate files
    // This file only contains the PaymentTransaction model

    public class PaymentTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TransactionId { get; set; } = string.Empty;

        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "USD";

        [StringLength(50)]
        public string PaymentMethod { get; set; } = "Credit Card";

        [StringLength(20)]
        public string? CardType { get; set; }

        [StringLength(4)]
        public string? Last4Digits { get; set; }

        [StringLength(20)]
        public string Status { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        [StringLength(100)]
        public string? ProcessorResponse { get; set; }

        [StringLength(200)]
        public string? BillingAddress { get; set; }

        [StringLength(50)]
        public string? BillingCity { get; set; }

        [StringLength(50)]
        public string? BillingState { get; set; }

        [StringLength(10)]
        public string? BillingZipCode { get; set; }

        [StringLength(50)]
        public string? BillingCountry { get; set; }
    }
}
