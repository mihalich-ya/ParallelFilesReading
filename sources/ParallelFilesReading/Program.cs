using System.Diagnostics;

namespace ParallelFilesReading;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Begin spaces count");

        var basePath = $"{AppDomain.CurrentDomain.BaseDirectory}/TestFiles";
        var tasks = new List<Task>();
        var sw = new Stopwatch();
        sw.Start();

        // Делаем три таски
        for (int i = 1; i <= 3; i++)
        {
            var filePath = $"{basePath}/TextFile{i}.txt";
            var task = Task.Run(() => CountSpacesInFile(filePath));
            tasks.Add(task);
        }

        Task.WaitAll(tasks);

        Console.WriteLine("\nCount spaces in folder.\n");

        // Считаем пробелы во всех файлах в папке
        CountSpacesInFolder(basePath);

        sw.Stop();
        Console.WriteLine("End spaces count. Elapsed (ms): {0}", sw.ElapsedMilliseconds);
    }

    static void CountSpacesInFile(string filePath)
    {
        // Т.к. файлы небольшие (для тестов), то просто весь файл читаем в строку.
        var content = File.ReadAllText(filePath);
        var spacesCount = content.Count(@char => @char == ' ');

        Console.WriteLine("Spaces count: {0} in file: {1}", spacesCount, Path.GetFileName(filePath));
    }

    static void CountSpacesInFolder(string path)
    {
        var tasks = new List<Task>();

        var files = Directory.GetFiles(path);
        foreach (var file in files)
        {
            var task = Task.Run(() => CountSpacesInFile(file));
            tasks.Add(task);
        }

        Task.WaitAll(tasks);
    }
}
