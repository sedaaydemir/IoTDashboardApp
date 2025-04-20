using IoTDashboardApp.Hubs;
using IoTDashboardApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();//sess�on eklend� kullan�lmak �c�n
builder.Services.AddSignalR();//signalR ayar�
builder.Services.AddSingleton<ModbusService>();//modbus serv�s� ekled�k bu serv�s� uygulaman�n her yer�nde kullanab�leceg�z
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

app.UseSession();//session akt�flend�
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");//ilk basta log�n sayfas� ac�ls�n

app.MapHub<DashboardHub>("/dashboardHub"); // DashboardHub i�in route
app.MapHub<PlcHub>("/plcHub"); // PlcHub i�in ayr� bir route
app.Run();
