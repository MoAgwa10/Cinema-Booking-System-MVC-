using Cinema_Booking_System.Models;

public interface IOrdersService
{
    Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
    Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);
    Task<Order?> GetLatestOrderByUserIdAsync(string userId);
    Task UpdateOrderAsync(Order order);
}
