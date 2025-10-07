using Cinema_Booking_System.Models;
using System.Text.RegularExpressions;

namespace Cinema_Booking_System.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
        CardType DetectCardType(string cardNumber);
        bool ValidateCardNumber(string cardNumber);
        bool ValidateExpiryDate(int month, int year);
        bool ValidateCVV(string cvv, CardType cardType);
        Task<PaymentResult> ValidatePaymentRequestAsync(PaymentRequest request);
    }

    public class MockPaymentService : IPaymentService
    {
        public async Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
        {
            // Validate payment request first
            var validationResult = await ValidatePaymentRequestAsync(request);
            if (!validationResult.Success)
            {
                return validationResult;
            }

            // Simulate processing time (realistic delay)
            await Task.Delay(Random.Shared.Next(2000, 4000));

            var result = new PaymentResult
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Last4Digits = request.CardNumber.Substring(request.CardNumber.Length - 4),
                CardType = DetectCardType(request.CardNumber).ToString(),
                PaymentMethod = "Credit Card",
                RiskLevel = "Low",
                RiskScore = 0.1,
                IsRefundable = true,
                RefundableAmount = request.Amount
            };

            // Always successful payment - just show success message
            result.Success = true;
            result.TransactionId = GenerateTransactionId();
            result.Message = "تم الدفع بنجاح! شكراً لك.";
            result.ProcessedAt = DateTime.UtcNow;
            result.AuthorizationCode = GenerateAuthorizationCode();
            result.ReceiptNumber = GenerateReceiptNumber();
            result.ProcessorResponse = "APPROVED";

            return result;
        }

        public CardType DetectCardType(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return CardType.Unknown;

            // Remove spaces and dashes
            cardNumber = Regex.Replace(cardNumber, @"[\s\-]", "");

            if (Regex.IsMatch(cardNumber, @"^4[0-9]{12}(?:[0-9]{3})?$"))
                return CardType.Visa;
            
            if (Regex.IsMatch(cardNumber, @"^5[1-5][0-9]{14}$"))
                return CardType.MasterCard;
            
            if (Regex.IsMatch(cardNumber, @"^3[47][0-9]{13}$"))
                return CardType.AmericanExpress;
            
            if (Regex.IsMatch(cardNumber, @"^6(?:011|5[0-9]{2})[0-9]{12}$"))
                return CardType.Discover;

            return CardType.Unknown;
        }

        public bool ValidateCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
                return false;

            // Remove spaces and dashes
            cardNumber = Regex.Replace(cardNumber, @"[\s\-]", "");

            // Check if all characters are digits
            if (!Regex.IsMatch(cardNumber, @"^\d+$"))
                return false;

            // Luhn algorithm validation
            return IsValidLuhn(cardNumber);
        }

        public bool ValidateExpiryDate(int month, int year)
        {
            if (month < 1 || month > 12)
                return false;

            var currentDate = DateTime.Now;
            var expiryDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            
            return expiryDate >= currentDate;
        }

        private bool IsValidLuhn(string cardNumber)
        {
            int sum = 0;
            bool alternate = false;

            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumber[i].ToString());

                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit = (digit % 10) + 1;
                }

                sum += digit;
                alternate = !alternate;
            }

            return (sum % 10) == 0;
        }

        private string GenerateTransactionId()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var random = Random.Shared.Next(1000, 9999);
            return $"TXN_{timestamp}_{random}";
        }

        private string GenerateAuthorizationCode()
        {
            var random = Random.Shared.Next(100000, 999999);
            return $"AUTH_{random}";
        }

        private string GenerateReceiptNumber()
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var random = Random.Shared.Next(100, 999);
            return $"RCP_{timestamp}_{random}";
        }

        public bool ValidateCVV(string cvv, CardType cardType)
        {
            if (string.IsNullOrEmpty(cvv))
                return false;

            // Remove any non-digit characters
            cvv = Regex.Replace(cvv, @"[^0-9]", "");

            // Check CVV length based on card type
            switch (cardType)
            {
                case CardType.AmericanExpress:
                    return cvv.Length == 4;
                case CardType.Visa:
                case CardType.MasterCard:
                case CardType.Discover:
                default:
                    return cvv.Length == 3;
            }
        }

        public async Task<PaymentResult> ValidatePaymentRequestAsync(PaymentRequest request)
        {
            var result = new PaymentResult
            {
                Amount = request.Amount,
                Currency = request.Currency,
                Success = false
            };

            // Validate card number
            if (!ValidateCardNumber(request.CardNumber))
            {
                result.Message = "Invalid card number. Please check and try again.";
                result.ErrorCode = "INVALID_CARD_NUMBER";
                return result;
            }

            // Validate expiry date
            if (!ValidateExpiryDate(request.ExpiryMonth, request.ExpiryYear))
            {
                result.Message = "Card has expired or invalid expiry date.";
                result.ErrorCode = "INVALID_EXPIRY_DATE";
                return result;
            }

            // Validate CVV
            var cardType = DetectCardType(request.CardNumber);
            if (!ValidateCVV(request.CVV, cardType))
            {
                result.Message = "Invalid CVV. Please check your security code.";
                result.ErrorCode = "INVALID_CVV";
                return result;
            }

            // Validate amount
            if (request.Amount <= 0)
            {
                result.Message = "Invalid payment amount.";
                result.ErrorCode = "INVALID_AMOUNT";
                return result;
            }

            // Validate cardholder name
            if (string.IsNullOrWhiteSpace(request.CardholderName))
            {
                result.Message = "Cardholder name is required.";
                result.ErrorCode = "MISSING_CARDHOLDER_NAME";
                return result;
            }

            // All validations passed
            result.Success = true;
            result.Message = "Payment request is valid.";
            
            return await Task.FromResult(result);
        }
    }
}
