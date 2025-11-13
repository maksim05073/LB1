using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace LB10
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            string firstFilePath = "firstFile.txt";
            
            if (!File.Exists(firstFilePath))
            {
                Console.WriteLine($"Помилка: Файл '{firstFilePath}' не знайдено!");
                Console.WriteLine("Будь ласка, створіть цей файл та додайте до нього назви текстових файлів для аналізу.");
                Console.ReadKey();
                return;
            }

            string[] fileNames = File.ReadAllLines(firstFilePath);
            if (fileNames.Length == 0)
            {
                Console.WriteLine($"Файл '{firstFilePath}' порожній. Додайте назви файлів для аналізу.");
                Console.ReadKey();
                return;
            }

            bool continueAnalysis = true;
            while (continueAnalysis)
            {
                Console.WriteLine("\n=== Доступні файли для аналізу ===");
                for (int i = 0; i < fileNames.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {fileNames[i]}");
                }
                
                int choice = 0;
                bool validChoice = false;
                while (!validChoice)
                {
                    Console.Write("Оберіть файл за номером (або '0' для виходу): ");
                    string input = Console.ReadLine();
                    if (int.TryParse(input, out choice))
                    {
                        if (choice == 0)
                        {
                            continueAnalysis = false;
                            break;
                        }
                        if (choice >= 1 && choice <= fileNames.Length)
                        {
                            validChoice = true;
                        }
                        else
                        {
                            Console.WriteLine($"Невірний номер. Будь ласка, введіть число від 1 до {fileNames.Length}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Невірний ввід. Будь ласка, введіть число.");
                    }
                }

                if (!continueAnalysis)
                {
                    break;
                }


                string selectedFile = fileNames[choice - 1];
                if (!File.Exists(selectedFile))
                {
                    Console.WriteLine($"Помилка: Файл '{selectedFile}' не знайдено! Перевірте шлях у firstFile.txt.");
                    continue;
                }

                Console.WriteLine($"\nАналіз файлу: {selectedFile}");
                string text = File.ReadAllText(selectedFile);
                
                var wordStats = AnalyzeText(text);
                
                Console.WriteLine("\n=== Статистика слів ===");
                if (wordStats.Count == 0)
                {
                    Console.WriteLine("Файл порожній або не містить слів.");
                }
                else
                {
                    foreach (var pair in wordStats)
                    {
                        Console.WriteLine($"{pair.Key,-20} : {pair.Value}");
                    }
                }
                
                Console.Write("\nЗберегти результати у файл? (y/n): ");
                if (Console.ReadLine()?.Trim().ToLower() == "y")
                {
                    string outputPath = Path.GetFileNameWithoutExtension(selectedFile) + "_stats.txt";
                    SaveStatsToFile(outputPath, wordStats);
                    Console.WriteLine($"Результати збережено у файл: {outputPath}");
                }
                
                Console.Write("\nБажаєте проаналізувати інший файл? (y/n): ");
                continueAnalysis = Console.ReadLine()?.Trim().ToLower() == "y";
            }

            Console.WriteLine("\nАналіз завершено. Натисніть будь-яку клавішу для виходу.");
            Console.ReadKey();
        }
        static Dictionary<string, int> AnalyzeText(string text)
        {
            char[] delimiters = new char[] { 
                ' ', ',', '.', '!', '?', ';', ':', '\n', '\r', '\t', 
                '\"', '(', ')', '[', ']', '{', '}', '<', '>', '/', '\\',
                '-', '—', '–', '`', '~', '@', '#', '$', '%', '^', '&', 
                '*', '_', '=', '+', '|'
            };
            var words = text
                .ToLower()
                .Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            var wordCounts = new Dictionary<string, int>();

            foreach (var word in words)
            {
                if (wordCounts.ContainsKey(word))
                {
                    wordCounts[word]++;
                }
                else
                {
                    wordCounts[word] = 1;
                }
            }
            return wordCounts.OrderByDescending(w => w.Value)
                             .ToDictionary(w => w.Key, w => w.Value);
        }
        static void SaveStatsToFile(string path, Dictionary<string, int> stats)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
                {
                    writer.WriteLine("--- Статистика слів ---");
                    if (stats.Count == 0)
                    {
                        writer.WriteLine("Слів не знайдено.");
                    }
                    else
                    {
                        foreach (var pair in stats)
                        {
                            writer.WriteLine($"{pair.Key,-20} : {pair.Value}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження файлу: {ex.Message}");
            }
        }
    }
}