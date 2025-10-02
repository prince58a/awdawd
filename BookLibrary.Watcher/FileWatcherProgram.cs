using System;
using System.IO;
using System.Threading;

namespace BookLibrary.Watcher
{
    class FileWatcherProgram
    {
        private FileSystemWatcher watcher;

        public void StartWatching(string path)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Filter = "*.json"; // или другие файлы данных
            watcher.NotifyFilter = NotifyFilters.LastWrite;

            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            Console.WriteLine($"Наблюдение за папкой: {path}");
            Console.WriteLine("Нажмите 'q' для выхода");

            while (Console.Read() != 'q') ;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine($"Файл изменен: {e.FullPath} в {DateTime.Now}");
            // Здесь можно добавить логику обработки изменений
        }
    }
}