using EasyModbus;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace IoTDashboardApp.Services
{
    public class ModbusService
    {
        //modbus baglantı tanımı
        private ModbusClient _modbusClient;

        public ModbusService()
        {
            _modbusClient = new ModbusClient("127.0.0.1", 502); // IP ve Port 192.168.3.250
        }


        // Modbus baglantısı baslat
        public async Task<bool> ConnectAsync()
        {
            try
            {
                _modbusClient.ConnectionTimeout = 5000;
                _modbusClient.Connect();

                return _modbusClient.Connected;
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                //consolda baglantıyı kontrol edelım
                Console.WriteLine($"Bağlantı hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // dıgerdurumları gorelım
                Console.WriteLine($"Beklenmedik hata: {ex.Message}");
                return false;
            }
        }

        //public async Task<bool> TestConnection()
        //{
        //    bool isConnected = await ConnectAsync();
        //    if (isConnected)
        //    {
        //        Console.WriteLine("Bağlantı başarılı.");
        //        return true;
        //    }
        //    else
        //    {
        //        Console.WriteLine("Bağlantı hatası.");
        //        return false;
        //    }
        //}
        // Read modbus
        public async Task<float> ReadTemperatureAsync()
        {
            try
            {
                int[] values = await Task.Run(() => _modbusClient.ReadHoldingRegisters(1, 2));

                if (values.Length >= 2)
                {
                    // float D66 adresinden veri okuma

                    short temperature = (short)values[0]; // signed dönüşüm
                    float result = temperature / 10f;
                    Console.WriteLine($"Okunan sıcaklık: {result} °C"); // Debug için log
                    return result;
                }
                else
                {
                    Console.WriteLine("Veri okunamadı.");
                    return 0f; // Eğer okuma yapılmazsa, 0 döndür
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Okuma hatası: {ex.Message}");
                return 0f;
            }
        }

        //// Write modbus
        //public async Task WriteDataAsync(int register, int value)
        //{
        //    try
        //    {
        //        await Task.Run(() => _modbusClient.WriteSingleRegister(register, value));
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Yazma hatası: {ex.Message}");
        //    }
        //}

        // Bağlantıyı kesme durumu
        public void Disconnect()
        {
            _modbusClient.Disconnect();
        }
    }
}

