using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace BookLibrary.Watcher
{
    class FileWatcherProgram
    {
        private static readonly int Port = 5555;
        private static List<TcpClient> clients = new List<TcpClient>();
        private static FileSystemWatcher watcher;
        private static readonly object lockObj = new();

        static void Main(string[] args)
        {
            Console.WriteLine("=== BookLibrary.Watcher ===");
            Console.WriteLine("Сервер уведомлений запущен.");

            string dataFile = @"C:\Users\egorg\Documents\GitHub\awdawd\books_data.json";
            StartFileWatcher(dataFile);
            StartTcpServer();
        }

        private static void StartFileWatcher(string filePath)
        {
            watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath), Path.GetFileName(filePath))
            {
                NotifyFilter = NotifyFilters.LastWrite
            };
            watcher.Changed += (s, e) =>
            {
                Thread.Sleep(150);
                Console.WriteLine($"📁 Файл обновлён: {e.FullPath}");
                BroadcastMessage("DATA_CHANGED");
            };
            watcher.EnableRaisingEvents = true;
        }

        private static void StartTcpServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, Port);
            listener.Start();
            Console.WriteLine($"🟢 TCP-сервер слушает порт {Port}");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                lock (lockObj) clients.Add(client);
                Console.WriteLine("🔌 Клиент подключён");

                new Thread(() => HandleClient(client)).Start();
            }
        }

        private static void HandleClient(TcpClient client)
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
                    Console.WriteLine($"📨 От клиента: {msg}");

                    if (msg == "DATA_CHANGED")
                    {
                        BroadcastMessage("DATA_CHANGED", client);
                    }
                }
            }
            catch { }
            finally
            {
                lock (lockObj) clients.Remove(client);
                client.Close();
                Console.WriteLine("❌ Клиент отключён");
            }
        }

        private static void BroadcastMessage(string message, TcpClient? except = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            lock (lockObj)
            {
                foreach (var c in clients.ToArray())
                {
                    if (c == except) continue;
                    try
                    {
                        c.GetStream().Write(data, 0, data.Length);
                    }
                    catch { clients.Remove(c); }
                }
            }
        }
    }
}
