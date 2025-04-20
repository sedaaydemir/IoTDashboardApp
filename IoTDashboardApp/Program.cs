using IoTDashboardApp.Hubs;
using IoTDashboardApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();//sessýon eklendý kullanýlmak ýcýn
builder.Services.AddSignalR();//signalR ayarý
builder.Services.AddSingleton<ModbusService>();//modbus servýsý ekledýk bu servýsý uygulamanýn her yerýnde kullanabýlecegýz
builder.Services.AddHostedService<PlcWorker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();//session aktýflendý
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");//ilk basta logýn sayfasý acýlsýn

app.MapHub<DashboardHub>("/dashboardHub"); // DashboardHub için route
app.MapHub<PlcHub>("/plcHub"); // PlcHub için ayrý bir route
app.Run();
