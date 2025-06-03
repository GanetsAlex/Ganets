using Ganets.Blazor.Components;
using Ganets.Blazor.Services;
using Ganets.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHttpClient<IProductService<Gadget>, ApiProductService>(c =>
        c.BaseAddress = new Uri("https://localhost:7002/api/gadgets"));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
