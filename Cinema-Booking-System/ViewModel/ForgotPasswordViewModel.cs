using System.ComponentModel.DataAnnotations;

namespace Cinema_Booking_System.ViewModel
{

    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}