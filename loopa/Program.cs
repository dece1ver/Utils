﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace loopa
{
    class Program
    {
        static bool loading = false;

        static void Main(string[] args)
        {

            string targetDir;
            if (args.Length == 1 && Directory.Exists(args[0]))
            {
                targetDir = args[0];
            }
            else if (args.Length == 1 && (args[0] == "/?"))
            {
                Console.Clear();
                Console.WriteLine("Ищет файлы по указанному содержимому.\n" +
                    "Если просто запустить, то будет искать в папке с собой и во всех вложенных каталогах.\n" +
                    "Также можно перетащить нужную папку на этот файл, тогда будет искать в ней и всех вложенных каталогах.");
                Console.Write("\nДля продолжения нажмите любую клавишу...");
                Console.ReadKey();
                targetDir = Directory.GetCurrentDirectory();
            }
            else
            {
                targetDir = Directory.GetCurrentDirectory();
            }


            string[] files;
            string searchTarget = string.Empty;


            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Поиск в \"{targetDir}\"");
                Console.WriteLine("Что ищем? (справка /?)");
                searchTarget = Console.ReadLine();
                if (searchTarget == "/?")
                {
                    Console.Clear();
                    Console.WriteLine("Ищет файлы по указанному содержимому.\n" +
                        "Если просто запустить, то будет искать в папке с собой и во всех вложенных каталогах.\n" +
                        "Также можно перетащить нужную папку на этот файл, тогда будет искать в ней и всех вложенных каталогах.");
                    Console.Write("\nДля продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    continue;
                }
                else if (!string.IsNullOrEmpty(searchTarget))
                {
                    break;
                }
            }
            try
            {
                Console.Clear();
                Console.WriteLine($"Поиск \"{searchTarget}\" во всех файлах в указанной и всех вложенных папках.");
                //Console.Write("Подсчет файлов  ");
                Thread load = new(Loading);
                loading = true;
                Stopwatch stopWatch = new();
                stopWatch.Start();
                load.Start();
                files = Directory.GetFiles(targetDir, "*.*", SearchOption.AllDirectories);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                loading = false;
                int filesCount = files.Length;
                int filesFound = 0;
                string[] goodFiles = new string[filesCount];

                Console.WriteLine($"\rПодсчет файлов завершен. Всего файлов: {filesCount}. Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                stopWatch.Restart();
                for (int i = 0; i < filesCount; i++)
                {
                    ts = stopWatch.Elapsed;
                    Console.Write($"\rПрочитано файлов: {i}. Из них подходящих: {filesFound}. Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                    if (File.ReadAllText(files[i]).Contains(searchTarget))
                    {
                        goodFiles[filesFound] = files[i];
                        filesFound++;
                    }
                }
                stopWatch.Stop();
                ts = stopWatch.Elapsed;
                Console.WriteLine($"\rЧтение файлов завершено. Из них подходящих: {filesFound} Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.\n");
                Console.WriteLine(String.Join(Environment.NewLine, goodFiles).Trim());

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                Console.WriteLine("\nНажмите любую клавишу, чтобы закрыть это окно...");
                Console.ReadKey();
            }
        }


        private static void Loading()
        {
            int i = 0;
            Stopwatch stopWatch = new();
            stopWatch.Start();
            while (loading)
            {
                i++;
                Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
                TimeSpan ts = stopWatch.Elapsed;
                switch (i)
                {
                    case 1:
                        Console.Write($"\rПодсчет файлов | Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                        break;
                    case 2:
                        Console.Write($"\rПодсчет файлов / Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                        break;
                    case 3:
                        Console.Write($"\rПодсчет файлов - Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                        break;
                    default:
                        Console.Write($"\rПодсчет файлов \\ Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                        i = 0;
                        break;
                }
                Thread.Sleep(50);
            }
            stopWatch.Stop();
        }
    }
}
