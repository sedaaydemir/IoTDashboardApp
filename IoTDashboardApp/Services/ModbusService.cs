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



        // sureklı sıcaklık okuma
        public async Task<float> ReadTemperatureAsync()
        {
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

        //clıck ıle basınc okuma
        public async Task<float> ReadFromBasinc()
        {
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
        public int[] FloatToRegisters(float value)
        {
            // Float değeri byte dizisine dönüştür
            byte[] bytes = BitConverter.GetBytes(value);

            // Eğer Endian ters ise (BigEndian vs LittleEndian) buraya Array.Reverse() ekleyebilirsin
            Array.Reverse(bytes); // Eğer Endian problemi varsa

            // 2 adet int16 register'a bölüyoruz
            return new int[]
            {
        BitConverter.ToInt16(bytes, 0),
        BitConverter.ToInt16(bytes, 2)
            };
        }
        //plc ye verı yazma
        public async Task<bool> WriteFloatToPlcAsync(string address, float value)
        {
            try
            {
                // Float değeri yaz
                return await WriteFloatToPlcAsync(address, value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return false;
            }
        }


        // Bağlantıyı kesme durumu
        public void Disconnect()
        {
            _modbusClient.Disconnect();
        }
    }
}

