﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace FolderRenamer
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
            else
            {
                targetDir = Directory.GetCurrentDirectory();
            }
            string oldName = string.Empty;
            string newName = string.Empty;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Поиск в \"{targetDir}\"");
                Console.WriteLine("Что меняем? (справка /?)");
                oldName = Console.ReadLine();
                Console.WriteLine("На что меняем? (справка /?)");
                newName = Console.ReadLine();

                if (oldName == "/?" || newName == "/?")
                {
                    Console.Clear();
                    Console.WriteLine("Заменяет в названии всех найденных папок указанные значения.\n" +
                        "Если просто запустить, то будет искать и заменять в папке с собой и во всех вложенных каталогах.\n" +
                        "Также можно перетащить нужную папку на этот файл, тогда будет искать в ней и всех вложенных каталогах.");
                    Console.Write("\nДля продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    continue;
                }
                else if (!string.IsNullOrEmpty(oldName) || (!string.IsNullOrEmpty(newName)))
                {
                    Console.WriteLine($"Будет произведена замена \"{oldName}\" на \"{newName}\" во всех найденных папках\n" +
                        $"Для продолжения нажмите Enter, для повторного указания введите -");
                    
                    if (Console.ReadLine() == "-")
                    {
                        continue;
                    }
                    break;
                }
            }
            try
            {
                Console.Clear();
                Console.WriteLine($"Подсчет папок.");
                Thread load = new(Loading);
                loading = true;
                Stopwatch stopWatch = new();
                stopWatch.Start();
                load.Start();
                List<Folder> folders = new List<Folder>();
                string[] dirs = Directory
                                .EnumerateDirectories(targetDir, "*.*", SearchOption.AllDirectories)
                                .Where(dir => dir.Split(Path.DirectorySeparatorChar).Last().Contains(oldName))
                                .ToArray();
                for (int i = 0; i < dirs.Length; i++)
                {
                    folders.Add(new Folder { Path = dirs[i] });
                }
                List<Folder> sortedFolders = folders.OrderByDescending(x => x.Length).ToList();
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                loading = false;
                int dirsCount = folders.Count;
                int dirsRenamed = 0;
                int errors = 0;
                Console.Clear();
                Console.WriteLine($"\rПодсчет папок завершен. Подходящих: {dirsCount}. Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                stopWatch.Restart();
                for (int i = 0; i < dirsCount; i++)
                {
                    ts = stopWatch.Elapsed;
                    
                    string dirName = sortedFolders[i].Path.Split(Path.DirectorySeparatorChar).Last();
                    string dirRoot = sortedFolders[i].Path.Remove(sortedFolders[i].Path.LastIndexOf(Path.DirectorySeparatorChar));
                    string newDir = Path.Combine(dirRoot, dirName.Replace(oldName, newName));
                    try
                    {

                        Directory.Move(sortedFolders[i].Path, newDir);
                        dirsRenamed++;
                    }
                    catch(Exception e)
                    {
                        errors++;
                        Console.WriteLine($"\nНе удалось переместить:\n" +
                            $"с => {sortedFolders[i].Path}\n" +
                            $"в => {newDir}");
                    }
                    Console.Write($"\rПереименование: {i + 1} / {dirsCount}. Успешно: {dirsRenamed}. Ошибок: {errors} Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.");
                }
                stopWatch.Stop();
                ts = stopWatch.Elapsed;
                Console.WriteLine($"\rПереименовывание завершено. Переименовано: {dirsRenamed} / {dirsCount}. Затрачено времени: {(int)ts.TotalSeconds}.{ts.Milliseconds:D3}с.\n");
                Console.WriteLine($"Ошибок: {errors}");
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

        public static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);

            if (place == -1)
                return source;

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
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
    class Folder
        {
            public int Length { get => GetLength(Path);}
            public string Path { get; set; }

            private int GetLength(string path)
            {
                int count = 0;
                foreach (char c in path)
                    if (c == System.IO.Path.DirectorySeparatorChar) count++;
                return count;
            }
        }

    }
}
