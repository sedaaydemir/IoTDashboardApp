using EasyModbus;
using System;
using System.Threading.Tasks;

namespace IoTDashboardApp.Services
{
    public class ModbusService
    {
        //modbus baglantısı yapılacak
        //verı alıp verı dondurecek
        private ModbusClient _modbusClient;

        public ModbusService()
        {
            _modbusClient = new ModbusClient("127.0.0.1", 502); // IP ve Port bilgilerini burada güncelleyin
        }


        // Modbus cihazına bağlan
        public async Task<bool> ConnectAsync()
        {
            try
            {
                // Bağlantı zaman aşımını 5 saniye olarak ayarlıyoruz
                _modbusClient.ConnectionTimeout = 5000;

                // Bağlantıyı doğrudan başlatıyoruz
                _modbusClient.Connect();

                // Bağlantı başarılı ise true döndürüyoruz
                return _modbusClient.Connected;
            }
            catch (EasyModbus.Exceptions.ConnectionException ex)
            {
                // Bağlantı hatası durumunda özel mesajla hata yönetimi
                Console.WriteLine($"Bağlantı hatası: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                // Diğer hatalar için genel hata yönetimi
                Console.WriteLine($"Beklenmedik hata: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> TestConnection()
        {
            bool isConnected = await ConnectAsync();
            if (isConnected)
            {
                Console.WriteLine("Bağlantı başarılı.");
                return true;
            }
            else
            {
                Console.WriteLine("Bağlantı hatası.");
                return false;
            }
        }
        // Modbus cihazından sıcaklık verisini oku
        public async Task<float> ReadTemperatureAsync()
        {
            try
            {
                // Holding register'dan sıcaklık değeri oku (örneğin register 1'den)
                int[] values = await Task.Run(() => _modbusClient.ReadHoldingRegisters(66, 2));

                // 2 register'ı kullanarak sıcaklık değerini hesapla
                if (values.Length >=2 )
                {
                    // Örneğin sıcaklık değeri iki register'da birleştirilmiş olabilir
                    short temperature = (short)values[0]; // signed dönüşüm
                    float result = temperature / 10f;
                    return result;
                }

                else
                {
                    Console.WriteLine("Veri okunamadı.");
                    return 0f;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Okuma hatası: {ex.Message}");
                return 0f;
            }
        }

        // Modbus cihazından veri yaz
        public async Task WriteDataAsync(int register, int value)
        {
            try
            {
                await Task.Run(() => _modbusClient.WriteSingleRegister(register, value));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Yazma hatası: {ex.Message}");
            }
        }

        // Bağlantıyı kes
        public void Disconnect()
        {
            _modbusClient.Disconnect();
        }
    }
}

