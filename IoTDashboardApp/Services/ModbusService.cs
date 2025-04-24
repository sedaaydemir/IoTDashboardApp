using EasyModbus;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace IoTDashboardApp.Services
{
    public class ModbusService : IModbusService
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
                if (!_modbusClient.Connected) // ← EKLENDİ
                {
                    _modbusClient.ConnectionTimeout = 5000;
                    _modbusClient.Connect();
                }

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


        //sıcaklık degerını okuma
        public async Task<float> ReadTemperatureAsync()
        {

            //if (!_modbusClient.Connected)
            //    await ConnectAsync(); 
            try
            {
                int[] values = await Task.Run(() => _modbusClient.ReadHoldingRegisters(1, 2));

                if (values.Length >= 2)
                {
                    // float D1 adresinden veri okuma

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

        //Basınc degerını okuma
        public async Task<float> ReadFromBasinc()
        {
            //if (!_modbusClient.Connected)
            //    await ConnectAsync();

            try
            {
                int[] values = await Task.Run(() => _modbusClient.ReadHoldingRegisters(3, 2));

                if (values.Length >= 2)
                {
                    // float D1 adresinden veri okuma

                    short pressure = (short)values[0]; // signed dönüşüm
                    float result = pressure / 10f;
                    Console.WriteLine($"Okunan basınc: {result} °C"); // Debug için log
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



        //seviyeleri okuma
        public async Task<float> ReadLevelAsync()
        {
            //if (!_modbusClient.Connected)
            //    await ConnectAsync();

            try
            {
                int[] values = await Task.Run(() => _modbusClient.ReadHoldingRegisters(5, 2));

                if (values.Length >= 2)
                {
                    // float D1 adresinden veri okuma

                    short level = (short)values[0]; // signed dönüşüm
                    float result = level / 10f;
                    Console.WriteLine($"Okunan basınc: {result} lt"); // Debug için log
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



        ////plc ye verı yazma
        public Task<bool> WriteLevelAsync(float value)
        {
            try
            {
                int address = 7;
                byte[] bytes = BitConverter.GetBytes(value);

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                int high = (bytes[0] << 8) | bytes[1];
                int low = (bytes[2] << 8) | bytes[3];

                int[] data = new int[] { high, low };

                _modbusClient.WriteMultipleRegisters(address, data); // ← await yok

                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return Task.FromResult(false);
            }
        }


    }
}

