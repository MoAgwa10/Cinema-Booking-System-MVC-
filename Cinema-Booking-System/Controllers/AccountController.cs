using System.Text;
using System.Text.Encodings.Web;
using Cinema_Booking_System.Email;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Cinema_Booking_System.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

    // Register (GET)
    public IActionResult Register() => View();

    // Register (POST)
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
            return View(model);
        }

        // Assign User role to new registrations
        await _userManager.AddToRoleAsync(user, "User");

        // For development: Auto-confirm email (remove this in production)
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        await _userManager.ConfirmEmailAsync(user, token);

        // TODO: Enable email confirmation in production
        // var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        // var callbackUrl = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token = encodedToken }, Request.Scheme);
        // var html = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
        // await _emailSender.SendEmailAsync(model.Email, "Confirm your email", html);

        return RedirectToAction("Login");
    }

    public IActionResult RegisterConfirmation() => View();

    // Confirm Email
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null) return RedirectToAction("Index", "Home");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (result.Succeeded) return View("ConfirmEmail");
        return View("Error");
    }

    // Login (GET)
    public IActionResult Login(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // Login (POST)
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid) return View(model);

        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
        return View(model);
    }

    // Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Forgot Password (GET)
    public IActionResult ForgotPassword() => View();

    // Forgot Password (POST)
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Do not reveal that the user does not exist
            return RedirectToAction("ForgotPasswordConfirmation");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        var callbackUrl = Url.Action(nameof(ResetPassword), "Account", new { token = encodedToken, email = model.Email }, Request.Scheme);

        // TODO: Enable email sending in production
        // var html = $"Reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";
        // await _emailSender.SendEmailAsync(model.Email, "Reset Password", html);

        // For development: Show reset link directly (remove in production)
        TempData["ResetLink"] = callbackUrl;
        return RedirectToAction("ForgotPasswordConfirmation");
    }

    public IActionResult ForgotPasswordConfirmation() => View();

    // Reset Password (GET)
    public IActionResult ResetPassword(string token, string email)
    {
        var vm = new ResetPasswordViewModel { Token = token, Email = email };
        return View(vm);
    }

    // Reset Password (POST)
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Do not reveal that user does not exist
            return RedirectToAction("ResetPasswordConfirmation");
        }

        var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

        if (result.Succeeded) return RedirectToAction("ResetPasswordConfirmation");

        foreach (var e in result.Errors) ModelState.AddModelError("", e.Description);
        return View(model);
    }

    public IActionResult ResetPasswordConfirmation() => View();

    // Access Denied
    public IActionResult AccessDenied(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // Temporary method to assign User role to current user (for development)
    public async Task<IActionResult> AssignUserRole()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null && !await _userManager.IsInRoleAsync(user, "User"))
            {
                await _userManager.AddToRoleAsync(user, "User");
                TempData["Success"] = "User role assigned successfully! You can now access booking features.";
            }
            else
            {
                TempData["Info"] = "You already have the User role assigned.";
            }
        }
        else
        {
            TempData["Error"] = "You must be logged in to assign roles.";
        }
        
        return RedirectToAction("Index", "Home");
    }
    }
}