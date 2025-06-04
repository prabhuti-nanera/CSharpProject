using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace FileOrganizerAnalyzer
{
    
    struct FileSummary
    {
        public string Name;
        public long Size;
        public DateTime LastModified;
    }

    interface IFileProcessor
    {
        void ProcessFile(string filePath);
    }

    abstract class BaseProcessor
    {
        public abstract void Log(string message);
    }

    class TextFileProcessor : BaseProcessor, IFileProcessor
    {
        public event Action<string> OnFileProcessed;

        public void ProcessFile(string filePath)
        {
            Log($"[TextFileProcessor] Processing {filePath}");
            using (StreamReader reader = new StreamReader(filePath))
            {
                string content = reader.ReadToEnd();
                Console.WriteLine($"First 100 chars:\n{(content.Length > 100 ? content.Substring(0, 100) : content)}");
            }

            OnFileProcessed?.Invoke(filePath);
        }

        public override void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    class BinaryFileProcessor : BaseProcessor, IFileProcessor
    {
        public void ProcessFile(string filePath)
        {
            Log($"[BinaryFileProcessor] Processing {filePath}");
            using (BinaryReader br = new BinaryReader(File.Open(filePath, FileMode.Open)))
            {
                Console.WriteLine("First 10 bytes:");
                for (int i = 0; i < 10 && br.BaseStream.Position < br.BaseStream.Length; i++)
                {
                    Console.Write($"{br.ReadByte()} ");
                }
                Console.WriteLine();
            }
        }

        public override void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    class Program
    {
        class UserInput
        {
            public string GetDirectoryPath()
            {
                Console.Write("Enter directory path: ");
                return Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("=== File Organizer and Analyzer ===");

            UserInput input = new UserInput();
            string dirPath = input.GetDirectoryPath();

            if (!Directory.Exists(dirPath))
            {
                Console.WriteLine("Directory not found!");
                return;
            }

            string[] files = Directory.GetFiles(dirPath);
            Console.WriteLine("\nFiles:");
            foreach (var file in files)
                Console.WriteLine(file);

            List<FileSummary> summaries = new List<FileSummary>();

            var sortedFiles = files.OrderBy(f => f).ToList();

            Thread analysisThread = new Thread(() => AnalyzeFiles(sortedFiles, summaries));
            analysisThread.Start();

            Console.WriteLine("\n[Main] Application is responsive while files are analyzed in background.");
            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static void AnalyzeFiles(List<string> files, List<FileSummary> summaries)
        {
            Stack<IFileProcessor> processorStack = new Stack<IFileProcessor>();
            processorStack.Push(new TextFileProcessor());
            processorStack.Push(new BinaryFileProcessor());

            foreach (var file in files)
            {
                try
                {
                    string ext = Path.GetExtension(file).ToLower();
                    IFileProcessor processor = ext == ".txt" ? processorStack.Pop() : processorStack.Peek();

                    if (processor is TextFileProcessor textProcessor)
                    {
                        textProcessor.OnFileProcessed += f => Console.WriteLine($"[Event] {f} has been processed.");
                    }

                    processor.ProcessFile(file);

                    FileInfo fi = new FileInfo(file);
                    summaries.Add(new FileSummary
                    {
                        Name = Path.GetFileName(file),
                        Size = fi.Length,
                        LastModified = fi.LastWriteTime
                    });

                    StringBuilder sb = new StringBuilder();
                    sb.Append($"Summary: {fi.Name} | {fi.Length} bytes | Modified: {fi.LastWriteTime}");
                    Console.WriteLine(sb.ToString());

                    Thread.Sleep(500); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing {file}: {ex.Message}");
                }
            }

            Console.WriteLine("\n=== File Summaries ===");
            foreach (var summary in summaries)
            {
                Type type = summary.GetType();
                PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                Console.WriteLine($"Name: {summary.Name}, Size: {summary.Size} bytes, LastModified: {summary.LastModified}");
            }

            DateTime? earliestModified = summaries.Min(s => (DateTime?)s.LastModified);
            Console.WriteLine($"\nEarliest file modified date: {earliestModified?.ToString() ?? "N/A"}");
        }
    }
}
