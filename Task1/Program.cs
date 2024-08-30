using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AsyncFileProcessing
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var filePaths = new List<string>
            {
                @"/home/hackintosh/code/cSharp/file1.txt",
                @"/home/hackintosh/code/cSharp/file2.txt",
                @"/home/hackintosh/code/cSharp/file3.txt",
                @"/home/hackintosh/code/cSharp/file4.txt",
                // @"D:\data\file2.txt",
                // @"D:\data\file3.txt",
                // @"D:\data\file4.txt"
            };
            var results = await CalculateFileSumsAsync(filePaths);

            // await Task.Delay(1000);

            foreach (var result in results)
            {
                if(result.Value == -1)
                {
                    Console.WriteLine($"File: {result.Key}, Err: File not found");
                    continue;
                }
                Console.WriteLine($"File: {result.Key}, Sum: {result.Value}");
            }
        }
        public static async Task<Dictionary<string, int>> CalculateFileSumsAsync(List<string> filePaths)
        {
            var result = new Dictionary<string, int>();

            foreach (var filePath in filePaths)
            {
                try
                {
                    var sum = 0;
                    using (var reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            if (int.TryParse(line, out int number))
                            {
                                sum += number;
                            }
                        }
                    }

                    result[filePath] = sum;
                }
                catch (FileNotFoundException)
                {
                    result[filePath] = -1;
                }
                catch (IOException)
                {
                    result[filePath] = -2;
                }
            }

            return result;
        }
    }
}