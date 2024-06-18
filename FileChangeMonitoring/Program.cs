using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileMonitor
{
    class Program
    {
        private static string targetFilePath;
        private static string[] lastFileLines;
        private static Timer timer;

        static void Main(string[] args)
        {
            Console.WriteLine("Enter the full path of the target text file:");
            targetFilePath = Console.ReadLine();

            if (File.Exists(targetFilePath) && Path.GetExtension(targetFilePath) == ".txt")
            {
                Console.WriteLine("Monitoring started...");
                lastFileLines = File.ReadAllLines(targetFilePath);
                timer = new Timer(CheckFileChanges, null, 0, 15000);
                Console.WriteLine("Press [Enter] to exit the program.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid file. Please ensure the file exists and is a .txt file.");
            }
        }

        private static void CheckFileChanges(object state)
        {
            try
            {
                if (File.Exists(targetFilePath))
                {
                    string[] currentFileLines = File.ReadAllLines(targetFilePath);

                    if (!currentFileLines.SequenceEqual(lastFileLines))
                    {
                        Console.WriteLine("File has changed:");
                        Console.WriteLine("==== Changes Detected at " + DateTime.Now + " ====");

                        for (int i = 0; i < currentFileLines.Length; i++)
                        {
                            if (i >= lastFileLines.Length)
                            {
                                Console.WriteLine($"New line added at {i + 1}: {currentFileLines[i]}");
                            }
                            else if (currentFileLines[i] != lastFileLines[i])
                            {
                                Console.WriteLine($"Line {i + 1} updated: {currentFileLines[i]}");
                            }
                        }

                        if (currentFileLines.Length < lastFileLines.Length)
                        {
                            for (int i = currentFileLines.Length; i < lastFileLines.Length; i++)
                            {
                                Console.WriteLine($"Line {i + 1} removed");
                            }
                        }

                        Console.WriteLine("==============================================");
                        lastFileLines = currentFileLines;
                    }
                }
                else
                {
                    Console.WriteLine("Target file no longer exists.");
                    timer.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred while checking file changes: " + ex.Message);
            }
        }
    }

}