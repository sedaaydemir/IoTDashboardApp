using IoTDashboardApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace IoTDashboardApp.Services
{
    public class TempContReadService:BackgroundService
    {
        //bu servıs benım arkaplanda sureklı sıcaklıgı okuması ıcın gereklı olan servısım
        private readonly IHubContext<PlcHub> _hubContext;
        private readonly IModbusService _modbusService;
        public TempContReadService(IHubContext<PlcHub> hubContext, IModbusService modbusService)
        {
            _hubContext = hubContext;
            _modbusService = modbusService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var isConnected = await _modbusService.ConnectAsync();
                if (isConnected)
                {
                    float temp = await _modbusService.ReadTemperatureAsync();
                    await _hubContext.Clients.All.SendAsync("ReceiveTemperature", temp);
                }

                await Task.Delay(3000); // 3 saniyede bir sıcaklık yolla
            }
        }


    }
}
