namespace IoTDashboardApp.Services
{
    public interface IModbusService
    {
        Task<bool> ConnectAsync();
        Task<float> ReadLevelAsync();
        Task<float> ReadTemperatureAsync();
        Task<float> ReadFromBasinc();

        Task<bool> WriteLevelAsync(float value);


    }
}
