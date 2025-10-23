using System;
using System.Diagnostics;
using System.Threading;

namespace BookLibrary.Launcher
{
    class LauncherProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=========== ЛАУНЧЕР БИБЛИОТЕКИ ===========");
            Console.WriteLine("1. Запустить только консольное приложение");
            Console.WriteLine("2. Запустить только WinForms приложение");
            Console.WriteLine("3. Запустить оба приложения");
            Console.Write("Выберите вариант: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StartConsoleApp();
                    break;
                case "2":
                    StartWinFormsApp();
                    break;
                case "3":
                    StartBothApps();
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        static void StartConsoleApp()
        {
            StartAppInNewWindow("BookLibrary.ConsoleApp", "Консольная Библиотека");
            Console.WriteLine("Нажмите любую клавишу для возврата в лаунчер...");
            Console.ReadKey();
        }

        static void StartWinFormsApp()
        {
            StartAppInNewWindow("BookLibrary.WinForms", "WinForms Библиотека");
            Console.WriteLine("Нажмите любую клавишу для возврата в лаунчер...");
            Console.ReadKey();
        }

        static void StartBothApps()
        {
            try
            {
                string currentDir = Directory.GetCurrentDirectory();

                Console.WriteLine("Поиск и запуск проектов...");

                StartAppInNewWindow("BookLibrary.ConsoleApp", "Консольная Библиотека");

                Thread.Sleep(2000);

                StartAppInNewWindow("BookLibrary.WinForms", "WinForms Библиотека");

                Console.WriteLine("Оба приложения запущены!");
                Console.WriteLine("Лаунчер можно закрыть - приложения работают независимо");
                Console.WriteLine("Нажмите любую клавишу для закрытия этого окна...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void StartAppInNewWindow(string projectName, string windowTitle)
        {
            try
            {
                Console.WriteLine($"Запуск {projectName}...");

                // Вставь сюда свой к репозиторию
                //пк дима C:\Users\dshel\Документы\awdawd
                //ноут егор C:\Users\egorg\Documents\GitHub\awdawd
                string solutionDir = @"C:\Users\dshel\Документы\awdawd";
                string projectPath = Path.Combine(solutionDir, projectName, $"{projectName}.csproj");

                if (!File.Exists(projectPath))
                {
                    Console.WriteLine($"❌ Проект не найден: {projectPath}");
                    return;
                }

                string projectDir = Path.GetDirectoryName(projectPath);

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/k title {windowTitle} && dotnet run --project \"{projectPath}\"",
                        UseShellExecute = true,
                        CreateNoWindow = false,
                        WorkingDirectory = projectDir
                    }
                };

                process.Start();
                Console.WriteLine($"{projectName} запущен");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка запуска {projectName}: {ex.Message}");
            }
        }
    }
}