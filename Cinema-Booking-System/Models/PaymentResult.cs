namespace Cinema_Booking_System.Models
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? ErrorCode { get; set; }
        public string? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string? Last4Digits { get; set; }
        public string? CardType { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? ReceiptNumber { get; set; }
        
        // Additional metadata for transaction tracking
        public Dictionary<string, object>? Metadata { get; set; }
        
        // For refunds and partial payments
        public decimal? RefundableAmount { get; set; }
        public bool IsRefundable { get; set; } = true;
        
        // Payment method details
        public string? PaymentMethod { get; set; } = "Credit Card";
        public string? ProcessorResponse { get; set; }
        
        // Risk assessment (for fraud detection)
        public string? RiskLevel { get; set; } = "Low";
        public double? RiskScore { get; set; }
    }
}
