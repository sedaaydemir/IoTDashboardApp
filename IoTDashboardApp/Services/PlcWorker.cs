using IoTDashboardApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IoTDashboardApp.Services
{
    public class PlcWorker:BackgroundService
    {
        private readonly ModbusService _modbusService;
        private readonly IHubContext<PlcHub> _hubContext;

        public PlcWorker(ModbusService modbusService, IHubContext<PlcHub> hubContext)
        {
            _modbusService = modbusService;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _modbusService.ConnectAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                float temperature = await _modbusService.ReadTemperatureAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveTemperature", temperature);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
