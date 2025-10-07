using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cinema_Booking_System.Models
{
    public class Order
    {
        [Key]
        public int id { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        // Payment and Transaction Details
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        
        public string Currency { get; set; } = "USD";
        
        public string? TransactionId { get; set; }
        
        public string? PaymentStatus { get; set; } = "Pending"; // Pending, Completed, Failed, Refunded
        
        public string? PaymentMethod { get; set; } = "Credit Card";
        
        public string? Last4Digits { get; set; }
        
        public string? CardType { get; set; }
        
        public DateTime? PaymentProcessedAt { get; set; }
        
        public string? AuthorizationCode { get; set; }
        
        public string? ReceiptNumber { get; set; }
        
        // Order Status
        public string OrderStatus { get; set; } = "Processing"; // Processing, Confirmed, Cancelled, Completed
        
        // Billing Information
        public string? BillingAddress { get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Country { get; set; }
        
        // Additional Notes
        public string? Notes { get; set; }
        
        // Calculated Properties
        public bool IsPaymentCompleted => PaymentStatus == "Completed";
        public bool CanBeRefunded => IsPaymentCompleted && OrderStatus != "Cancelled";
    }
}