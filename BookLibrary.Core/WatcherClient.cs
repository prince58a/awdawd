using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BookLibrary.Core
{
    public class WatcherClient
    {
        private TcpClient client;
        private Thread listenThread;
        public event EventHandler DataChanged;

        public void Connect(string host = "127.0.0.1", int port = 5555)
        {
            try
            {
                client = new TcpClient(host, port);
                Console.WriteLine("Подключено к Watcher");

                listenThread = new Thread(Listen);
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка подключения к Watcher: {ex.Message}");
            }
        }

        private void Listen()
        {
            try
            {
                var stream = client.GetStream();
                var buffer = new byte[1024];
                while (true)
                {
                    int bytes = stream.Read(buffer, 0, buffer.Length);
                    if (bytes == 0) break;
                    string msg = Encoding.UTF8.GetString(buffer, 0, bytes);
                    if (msg == "DATA_CHANGED")
                    {
                        Console.WriteLine("[Watcher] Получено уведомление о смене данных");
                        DataChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch
            {
                Console.WriteLine("❌ Потеряно соединение с Watcher");
            }
        }

        public void NotifyChange()
        {
            try
            {
                if (client == null || !client.Connected) return;

                var stream = client.GetStream();
                byte[] data = Encoding.UTF8.GetBytes("DATA_CHANGED");
                stream.Write(data, 0, data.Length);
            }
            catch { }
        }
    }
}
