using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Parser
{
    public static class DataPipelineFactory
    {
        public static CustomFunctionPipeline GetCustomFunctionPipeline(int numberOfThreads = 1)
        {
            var result = new CustomFunctionPipeline(new DBParser());

            result.Tasks.Add(result.DBParser.Run());

            for (int j = 0; j < numberOfThreads; j++)
            {
                var cfParser = new CustomFunctionParser() { CustomFunctionKeys = result.DBParser.CustomFunctionKeys };
                result.CustomFunctionParsers.Add(cfParser);
                result.Tasks.Add(cfParser.Run());
            }
            return result;
        }

        public class CustomFunctionPipeline
        {
            public DBParser DBParser { get; private set; }
            public List<CustomFunctionParser> CustomFunctionParsers { get; private set; }
            public List<Task> Tasks { get; private set; }

            public CustomFunctionPipeline(DBParser dbParser)
            {
                DBParser = dbParser;
                CustomFunctionParsers = new List<CustomFunctionParser>();
                Tasks = new List<Task>();
            }
        }
    }
}
