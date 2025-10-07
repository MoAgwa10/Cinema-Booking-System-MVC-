using Cinema_Booking_System;
using Cinema_Booking_System.Data;
using Cinema_Booking_System.Data.Base;
using Cinema_Booking_System.Data.Repo;
using Cinema_Booking_System.Data.Service;
using Cinema_Booking_System.Email;
using Cinema_Booking_System.Models;
using Cinema_Booking_System.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinema_Booking_System
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("mycon"));
            });

            
            builder.Services.AddControllersWithViews();

            // Session configuration for shopping cart
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Services
            builder.Services.AddScoped<IActorsService, ActorsService>();
            builder.Services.AddScoped<IProducersService, ProducersService>();
            builder.Services.AddScoped<ICinemasService, CinemasService>();
            builder.Services.AddScoped<IMoviesService, MoviesService>();
            builder.Services.AddScoped<IOrdersService, OrdersService>();
            builder.Services.AddScoped(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddScoped<IPaymentService, MockPaymentService>();
            builder.Services.AddScoped<IImageService, ImageService>();

            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

           
            builder.Services.Configure<IdentityOptions>(options =>
            {
                
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                
                options.User.RequireUniqueEmail = true;
            });

            var app = builder.Build();

           
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await AppDbInitializer.SeedAsync(services);
            }

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
