using OrderManagement.Shared.Contracts;
using OrderManagement.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();
builder.Services.AddHttpClient("ChannelEngine").SetHandlerLifetime(TimeSpan.FromMinutes(3));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
