using CustomFunctionAnalysis_003.Parser;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Invalid arguments. Must supply a single database name");
                return;
            }

            Stopwatch clicker = new Stopwatch();

            clicker.Start();
            
            SingleThreaded(args[0]);
            // MultiThreaded(args[0]);

            clicker.Stop();
            Console.WriteLine("Elapsed time: {0} seconds", clicker.ElapsedMilliseconds / 1000);

            Console.Write("Press any key!");
            Console.ReadKey();
        }

        private static void SingleThreaded(string db)
        {
            var sourceConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["CustomFunctionsSource"].ConnectionString;
            var parser = new SourceCodeParser_SingleThread(db, sourceConnectionString, new[] {2, 2 });
            parser.Run();
        }

        private static void MultiThreaded(string db)
        {
            var pipeline = DataPipelineFactory.GetCustomFunctionPipeline(10);
            pipeline.DBParser.DatabaseNames.TryAdd(db);
            pipeline.DBParser.DatabaseNames.CompleteAdding();
            Task.WaitAll(pipeline.Tasks.ToArray());
        }
    }
}
