using CustomFunctionAnalysis_003.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Parser
{
    public class DBParser
    {
        public BlockingCollection<string> DatabaseNames { get; private set; }
        public BlockingCollection<CustomFunctionKey> CustomFunctionKeys { get; private set; }
        public CustomFunctionsTargetModel DataModel { get; set; }

        public DBParser()
        {
            DatabaseNames = new BlockingCollection<string>();
            CustomFunctionKeys = new BlockingCollection<CustomFunctionKey>();
            DataModel = new CustomFunctionsTargetModel();
        }

        public Task Run()
        {
            var task = Task.Factory.StartNew(() => {
                foreach (var dbName in DatabaseNames.GetConsumingEnumerable())
                {
                    var query = GetQuery(dbName);
                    foreach(var cf in query.ToList()) {
                        CustomFunctionKeys.TryAdd(cf);
                    }
                }
                CustomFunctionKeys.CompleteAdding();
            });

            return task;
        }

        private IQueryable<CustomFunctionKey> GetQuery(string dbName)
        {
            var query = from cf in DataModel.CustomFunctionDetails
                            //where cf.DB == dbName
                            select new CustomFunctionKey() {DB = /*cf.DB*/"test", FunctionID = cf.FunctionID};
            // return query.Take(40);
            return query;
        }        
    }

   
}
