using Ganets.UI.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

public class DbInit
{
    public static async Task SeedData(WebApplication application)
    {
        using var scope = application.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Проверяем, существует ли пользователь с email "admin@gmail.com"
        var user = await userManager.FindByEmailAsync("admin@gmail.com");
        if (user == null)
        {
            // Создаём нового пользователя
            user = new ApplicationUser();
            await userManager.SetEmailAsync(user, "admin@gmail.com");
            await userManager.SetUserNameAsync(user, user.Email);
            user.EmailConfirmed = true; // Имитация подтверждения email


            // Устанавливаем пароль администратора
            await userManager.CreateAsync(user, "123456"); 

            // Добавляем утверждение "role" со значением "admin"
            var claim = new Claim(ClaimTypes.Role, "admin");
            await userManager.AddClaimAsync(user, claim);
        }
    }
}
