using IoTDashboardApp.Hubs;
using IoTDashboardApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();//kullanýcý gýrýsý ýcýn sessýon eklendý

builder.Services.AddSignalR();//signalR ayarý
builder.Services.AddHostedService<TempContReadService>();
builder.Services.AddSingleton<IModbusService, ModbusService>();//IModbusSevirce nýn hangý sýnýfa karsýlýk geldýgýný býldýr

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

//app.MapHub<DashboardHub>("/dashboardHub"); // DashboardHub için route
app.MapHub<PlcHub>("/plcHub"); // PlcHub için ayrý bir route

app.Run();
