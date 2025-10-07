using Cinema_Booking_System.Cart;
using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Services;
using Cinema_Booking_System.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cinema_Booking_System.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IMoviesService _moviesService;
        private readonly IOrdersService _ordersService;
        private readonly IPaymentService _paymentService;

        public OrdersController(IMoviesService moviesService, IOrdersService ordersService, IPaymentService paymentService)
        {
            _moviesService = moviesService;
            _ordersService = ordersService;
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        [Authorize]
        public IActionResult ShoppingCart()
        {
            // Block admins from accessing shopping cart
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Administrators cannot access shopping cart. Please use a regular user account.";
                return RedirectToAction("AccessDenied", "Account");
            }

            var shoppingCart = Cinema_Booking_System.Cart.ShoppingCart.GetShoppingCart(HttpContext.RequestServices);
            var items = shoppingCart.GetShoppingCartItems();
            shoppingCart.ShoppingCartItems = items;

            var response = new ShoppingCartVM()
            {
                ShoppingCart = shoppingCart,
                ShoppingCartTotal = shoppingCart.GetShoppingCartTotal()
            };

            return View(response);
        }

        [Authorize]
        public async Task<IActionResult> AddItemToShoppingCart(int id)
        {
            // Block admins from booking
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Administrators cannot book tickets. Please use a regular user account.";
                return RedirectToAction("AccessDenied", "Account");
            }

            var item = await _moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                var shoppingCart = Cinema_Booking_System.Cart.ShoppingCart.GetShoppingCart(HttpContext.RequestServices);
                shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        [Authorize]
        public async Task<IActionResult> RemoveItemFromShoppingCart(int id)
        {
            // Block admins from booking
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Administrators cannot book tickets. Please use a regular user account.";
                return RedirectToAction("AccessDenied", "Account");
            }

            var item = await _moviesService.GetMovieByIdAsync(id);

            if (item != null)
            {
                var shoppingCart = Cinema_Booking_System.Cart.ShoppingCart.GetShoppingCart(HttpContext.RequestServices);
                shoppingCart.RemoveItemFromCart(item);
            }
            return RedirectToAction(nameof(ShoppingCart));
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            // Block admins from booking
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Administrators cannot book tickets. Please use a regular user account.";
                return RedirectToAction("AccessDenied", "Account");
            }

            var shoppingCart = Cinema_Booking_System.Cart.ShoppingCart.GetShoppingCart(HttpContext.RequestServices);
            var items = shoppingCart.GetShoppingCartItems();
            
            if (!items.Any())
            {
                TempData["Error"] = "Your shopping cart is empty.";
                return RedirectToAction(nameof(ShoppingCart));
            }

            var paymentRequest = new PaymentRequest
            {
                Amount = (decimal)shoppingCart.GetShoppingCartTotal(),
                Currency = "USD"
            };

            return View(paymentRequest);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProcessPayment(PaymentRequest paymentRequest)
        {
            // Block admins from booking
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Administrators cannot book tickets. Please use a regular user account.";
                return RedirectToAction("AccessDenied", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View("Checkout", paymentRequest);
            }

            var shoppingCart = Cinema_Booking_System.Cart.ShoppingCart.GetShoppingCart(HttpContext.RequestServices);
            var items = shoppingCart.GetShoppingCartItems();
            
            if (!items.Any())
            {
                TempData["Error"] = "Your shopping cart is empty.";
                return RedirectToAction(nameof(ShoppingCart));
            }

            // Set the correct amount from cart
            paymentRequest.Amount = (decimal)shoppingCart.GetShoppingCartTotal();

            try
            {
                // Process payment
                var paymentResult = await _paymentService.ProcessPaymentAsync(paymentRequest);
                
                if (paymentResult.Success)
                {
                    // Create order with payment details
                    string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    string userEmailAddress = User.FindFirstValue(ClaimTypes.Email);

                    // Create order first
                    await _ordersService.StoreOrderAsync(items, userId, userEmailAddress);
                    
                    // Get the created order to update it with payment details
                    var createdOrder = await _ordersService.GetLatestOrderByUserIdAsync(userId);
                    if (createdOrder != null)
                    {
                        // Update order with payment details
                        createdOrder.TotalAmount = paymentResult.Amount;
                        createdOrder.Currency = paymentResult.Currency;
                        createdOrder.TransactionId = paymentResult.TransactionId;
                        createdOrder.PaymentStatus = "Completed";
                        createdOrder.PaymentMethod = paymentResult.PaymentMethod ?? "Credit Card";
                        createdOrder.Last4Digits = paymentResult.Last4Digits;
                        createdOrder.CardType = paymentResult.CardType;
                        createdOrder.PaymentProcessedAt = paymentResult.ProcessedAt;
                        createdOrder.AuthorizationCode = paymentResult.AuthorizationCode;
                        createdOrder.ReceiptNumber = paymentResult.ReceiptNumber;
                        createdOrder.OrderStatus = "Confirmed";
                        createdOrder.BillingAddress = paymentRequest.BillingAddress;
                        createdOrder.City = paymentRequest.City;
                        createdOrder.PostalCode = paymentRequest.PostalCode;
                        createdOrder.Country = paymentRequest.Country;
                        
                        await _ordersService.UpdateOrderAsync(createdOrder);
                    }
                    
                    // Clear cart
                    await shoppingCart.ClearShoppingCartAsync();
                    
                    // Store payment result for confirmation page
                    TempData["PaymentResult"] = System.Text.Json.JsonSerializer.Serialize(paymentResult);
                    TempData["OrderId"] = createdOrder?.id;
                    
                    return View("OrderCompleted", paymentResult);
                }
                else
                {
                    TempData["Error"] = paymentResult.Message;
                    return View("Checkout", paymentRequest);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while processing your payment. Please try again.";
                return View("Checkout", paymentRequest);
            }
        }

        [Authorize]
        public async Task<IActionResult> CompleteOrder()
        {
            // Block admins from booking
            if (User.IsInRole("Admin"))
            {
                TempData["Error"] = "Administrators cannot book tickets. Please use a regular user account.";
                return RedirectToAction("AccessDenied", "Account");
            }

            // Redirect to new checkout flow
            return RedirectToAction(nameof(Checkout));
        }
    }
}
