using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace CmpOne
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var results = new Dictionary<string, HashSet<string>>();
            var analysis = new Analysis();
            analysis.Engage(results);

            var fileName = System.IO.Path.GetTempFileName();
            using (var writer = File.CreateText(fileName))
            {
                foreach (var result in results)
                {
                    if (result.Value.Count == 0) continue;
                    writer.WriteLine($"+++++++++++++++++++++ {result.Key}");
                    writer.WriteLine($"=============================================================");
                    foreach (var item in result.Value)
                    {
                        writer.WriteLine(item);
                    }
                    writer.WriteLine(Environment.NewLine);
                }
            }

            /*
            foreach (var result in results)
            {
                if (result.Value.Count == 0) continue;
                Console.WriteLine($"+++++++++++++++++++++ {result.Key}");
                Console.WriteLine($"=============================================================");
                foreach (var item in result.Value)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine(Environment.NewLine);
            }
            */

            Console.WriteLine($"Your results are here: {fileName}");

            Console.ReadKey();
        }
    }
}
