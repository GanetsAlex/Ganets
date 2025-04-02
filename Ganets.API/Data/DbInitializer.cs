using Ganets.API.Data;
using Ganets.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public static class DbInitializer
{
    public static async Task SeedData(WebApplication app)
    {
        // Uri проекта 
        var uri = "https://localhost:7002/";
        // Получение контекста БД 
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // Выполнение миграций
            await context.Database.MigrateAsync();
        // Заполнение данными 
        if (!context.Categories.Any() && !context.Gadgets.Any())
        {
            var categories = new Category[]
            {
            new Category { Name="Телефоны", NormalizedName="phones"},
            new Category { Name="Игровые приставки", NormalizedName="consoles"},
            new Category { Name="Умные часы", NormalizedName="watches"},
            };
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var gadgets = new List<Gadget>
            {
                new Gadget {Name="IPhone 16 pro max",
                    Description="Дорогая фигня, не отличается от прошлых",
                    Price =3999, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("phones")),
                    Image=uri+"Images/iphone.jpg" },
                new Gadget { Name="Samsung s25",
                    Description="Норм телефон, если бы не цена и батарея",
                    Price =2700, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("phones")),
                    Image=uri+"Images/samsung.jpg" },
                new Gadget {Name="Apple Watch s10",
                    Description="Норм часы, если бы не цена и батарея",
                    Price =1900, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("watches")),
                    Image=uri+"Images/applewatch.jpg" },
                new Gadget { Name="Samsung Watch Ultra",
                    Description="Ни дизайна, ни батареии и цена космос",
                    Price =1899, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("watches")),
                    Image=uri + "Images/samsungwatch.jpg"},
                new Gadget { Name="Плойка 5-ая",
                    Description="Если есть свободное время поможет его скоротать",
                    Price =2400, Category=categories.FirstOrDefault(c=>c.NormalizedName.Equals("consoles")),
                    Image=uri + "Images/ps5.jpg" }
            };
            await context.AddRangeAsync(gadgets);
            await context.SaveChangesAsync();

        }
    }
}