using System;
using System.Collections.Generic;
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

            Console.ReadKey();
        }
    }
}
