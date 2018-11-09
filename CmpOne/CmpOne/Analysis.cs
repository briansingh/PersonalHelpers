using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace CmpOne
{
    public class Analysis
    {
        private const string ConnectionString = "Server=DDCMCWPD2;Database=RaveDev;uid=RaveDev;pwd=password*8;Connection Timeout=600;Max Pool Size=300;MultipleActiveResultSets=true";

        private const string SqlForCfExecuteScalar = "SELECT URL, SourceCode FROM cmpone WHERE SourceCode LIKE '%CustomFunction.Database.ExecuteScalar%'";
        private const string PatternForCfExecuteScalar = "CustomFunction.Database.ExecuteScalar\\([.\\w]+,[ ]?([\"\\w]+),.+\\);";

        private const string SqlForCfExecuteDataSet = "SELECT URL, SourceCode FROM cmpone WHERE SourceCode LIKE '%CustomFunction.Database.ExecuteDataSet%'";
        private const string PatternForCfExecuteDataSet = "CustomFunction.Database.ExecuteDataSet\\([.\\w]+,[ ]?([\"\\w]+),.+\\);";

        private const string SqlForAgent = "SELECT URL, SourceCode FROM cmpone WHERE SourceCode LIKE '%CustomizationAgent%'";
        private const string PatternForAgentExecuteScalar = "CustomizationAgent.ExecuteScalar\\(([\" \\w]+),.+\\);";
        private const string PatternForAgentExecuteDataSet = "CustomizationAgent.ExecuteDataSet\\(([\" \\w]+),.+\\);";

        //#region DEBUG
        //private const string DebugSqlForCFDB = "SELECT URL, SourceCode FROM cmpone WHERE SourceCode LIKE '%CustomFunction.Database.%'";
        //private const string DebugPatternForCFDB = "CustomFunction.Database..+";
        //private const string DebugSqlForAgent = "SELECT URL, SourceCode FROM cmpone WHERE SourceCode LIKE '%CustomizationAgent.%'";
        //private const string DebugPatternForAgent = "CustomizationAgent..+";
        //#endregion

        public void Engage(Dictionary<string, HashSet<string>> results)
        {
            //#region DEBUG
            //DebugGetStoredProcsCFDB(results);
            //DebugGetStoredProcsAgent(results);
            //#endregion

            GetStoredProcsUsingCfExecuteScalar(results);
            GetStoredProcsUsingCfExecuteDataSet(results);
            GetStoredProcsUsingCustomizationAgent(results);
        }

        //#region DEBUG
        //private static void DebugGetStoredProcsCFDB(IDictionary<string, HashSet<string>> results)
        //{
        //    var customFunctions = GetListFromDatabase(DebugSqlForCFDB);
        //    GetStoredProcedures(results, customFunctions, DebugPatternForCFDB);
        //}
        //private static void DebugGetStoredProcsAgent(IDictionary<string, HashSet<string>> results)
        //{
        //    var customFunctions = GetListFromDatabase(DebugSqlForAgent);
        //    GetStoredProcedures(results, customFunctions, DebugPatternForAgent);
        //}
        //#endregion

        private static void GetStoredProcsUsingCfExecuteScalar(IDictionary<string, HashSet<string>> results)
        {
            var customFunctions = GetListFromDatabase(SqlForCfExecuteScalar);
            GetStoredProcedures(results, customFunctions, PatternForCfExecuteScalar);
        }

        private static void GetStoredProcsUsingCfExecuteDataSet(IDictionary<string, HashSet<string>> results)
        {
            var customFunctions = GetListFromDatabase(SqlForCfExecuteDataSet);
            GetStoredProcedures(results, customFunctions, PatternForCfExecuteDataSet);
        }
        private static void GetStoredProcsUsingCustomizationAgent(IDictionary<string, HashSet<string>> results)
        {
            var customFunctions = GetListFromDatabase(SqlForAgent);
            GetStoredProcedures(results, customFunctions, PatternForAgentExecuteScalar);
            GetStoredProcedures(results, customFunctions, PatternForAgentExecuteDataSet);
        }
        private static IEnumerable<CustomFunctionFromDatabase> GetListFromDatabase(string sql)
        {
            var results = new List<CustomFunctionFromDatabase>();
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(sql, conn))
                {
                    var reader = cmd.ExecuteReader();
                    if (reader == null || !reader.HasRows) return Enumerable.Empty<CustomFunctionFromDatabase>();
                    while (reader.Read())
                    {
                        var url = (string)reader[0];
                        var content = (string)reader[1];
                        var cf = new CustomFunctionFromDatabase
                        {
                            Url = url,
                            CustomFunctionContent = content
                        };
                        results.Add(cf);
                    }
                }
            }
            return results;
        }

        private static void GetStoredProcedures(IDictionary<string, HashSet<string>> results, IEnumerable<CustomFunctionFromDatabase> customFunctions, string pattern)
        {
            foreach (var customFunction in customFunctions)
            {
                var regex = new Regex(pattern);
                var cfStatements = regex.Matches(customFunction.CustomFunctionContent);
                if (cfStatements.Count == 0) continue;
                ExtractStoredProcedures(results, customFunction, cfStatements);
            }
        }

        private static void ExtractStoredProcedures(IDictionary<string, HashSet<string>> results, CustomFunctionFromDatabase customFunction, IEnumerable cfStatements)
        {
            foreach (Match cfStatement in cfStatements)
            {
                //#region DEBUG
                //MakeNoteOfStoredProcedure(results, customFunction, cfStatement.Value);
                //continue;
                //#endregion

                var storedProcName = cfStatement.Groups[1].Value;
                if (storedProcName.Contains("\""))
                {
                    MakeNoteOfStoredProcedure(results, customFunction, storedProcName);
                }
                else
                {
                    var spNameStatement = string.Format("{0}[ ]?=[ ]?\"([\\w]+)\";", storedProcName);
                    var regex = new Regex(spNameStatement);
                    var matches = regex.Matches(customFunction.CustomFunctionContent);
                    if (matches.Count == 0) continue;
                    foreach (Match match in matches)
                    {
                        var name = match.Groups[1].Value;
                        MakeNoteOfStoredProcedure(results, customFunction, name);
                    }
                }
            }
        }

        private static void MakeNoteOfStoredProcedure(IDictionary<string, HashSet<string>> results, CustomFunctionFromDatabase customFunction, string storedProcName)
        {
            var sp = storedProcName.Trim('"');
            if (results.ContainsKey(customFunction.Url))
            {
                results[customFunction.Url].Add(sp);
            }
            else
            {
                var list = new HashSet<string> {sp};
                results.Add(customFunction.Url, list);
            }
        }
    }
}
