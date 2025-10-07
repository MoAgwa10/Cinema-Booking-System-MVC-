using Cinema_Booking_System;
using Cinema_Booking_System.Models;
using Microsoft.EntityFrameworkCore;

public class OrdersService : IOrdersService
{
    private readonly AppDbContext _context;
    public OrdersService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
    {
        var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Movie).Include(n => n.User).ToListAsync();

        if (userRole != "Admin")
        {
            orders = orders.Where(n => n.UserId == userId).ToList();
        }

        return orders;
    }

    public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
    {
        var order = new Order()
        {
            UserId = userId,
            Email = userEmailAddress
        };
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();

        foreach (var item in items)
        {
            var orderItem = new OrderItem()
            {
                Amount = item.Amount,
                MovieId = item.Movie.id,
                OrderId = order.id,
                Price = item.Movie.Price
            };
            await _context.OrderItems.AddAsync(orderItem);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<Order?> GetLatestOrderByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateOrderAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}