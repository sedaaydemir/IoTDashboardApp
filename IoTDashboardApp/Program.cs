using IoTDashboardApp.Hubs;
using IoTDashboardApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();//kullan�c� g�r�s� �c�n sess�on eklend�

builder.Services.AddSignalR();//signalR ayar�
builder.Services.AddHostedService<TempContReadService>();
builder.Services.AddSingleton<IModbusService, ModbusService>();//IModbusSevirce n�n hang� s�n�fa kars�l�k geld�g�n� b�ld�r

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

app.UseSession();//session akt�flend�
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");//ilk basta log�n sayfas� ac�ls�n

//app.MapHub<DashboardHub>("/dashboardHub"); // DashboardHub i�in route
app.MapHub<PlcHub>("/plcHub"); // PlcHub i�in ayr� bir route

app.Run();
