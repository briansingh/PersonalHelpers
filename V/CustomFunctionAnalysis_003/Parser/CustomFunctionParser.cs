using CustomFunctionAnalysis_003.Model;
using CustomFunctionAnalysis_003.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Parser
{
    public class CustomFunctionParser
    {
        public BlockingCollection<CustomFunctionKey> CustomFunctionKeys { get; set; }
        public CustomFunctionsTargetModel DataModel { get; set; }

        public CustomFunctionParser()
        {
            DataModel = new CustomFunctionsTargetModel();
        }

        public Task Run()
        {
            var task = Task.Factory.StartNew(() => {
                if (CustomFunctionKeys == null)
                {
                    return;
                }

                foreach (var cfk in CustomFunctionKeys.GetConsumingEnumerable())
                {
                    var query = from cf in DataModel.CustomFunctionDetails
                                where /*cf.DB == cfk.DB &&*/ cf.FunctionID == cfk.FunctionID
                                select cf;

                    var customFunctions = query.ToList();
                    if (customFunctions.Count == 1)
                    {
                        // AnalyzeCustomFunction(customFunctions[0]);
                    }
                    else
                    {
                        // This is an error
                    }
                }
            });

            return task;
        }

        private void AnalyzeCustomFunction(CustomFunction cf)
        {
            
            Console.WriteLine(" - {0}:{1} ({2})", /*cf.DB*/"--", cf.FunctionID, cf.FunctionName);

            var sourceCode = GetSourceCode(cf);
            var sha256 = GenerateSHA256(sourceCode);
            var details = CreateDetailRecord(cf, "test");

            DataModel.CustomFunctionDetails.Add(details);
            DataModel.SaveChanges();

            var attributeEntity = new CustomFunctionAttribute()
            {
                //CustomFunction = details,
                SHA256 = sha256,
                //DB = cf.DB,
                //FunctionID = cf.FunctionID,
                MethodCalls = new List<MethodCall>()
            };

            DataModel.CustomFunctionAttributes.Add(attributeEntity);
            DataModel.SaveChanges();

            SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
            var root = (CompilationUnitSyntax)tree.GetRoot();
            var compilationUnit = CreateCompilationUnit(cf, tree);
            var model = compilationUnit.GetSemanticModel(tree);

            var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
            foreach (var method in methods)
            {
                var invocations = method.DescendantNodes().OfType<InvocationExpressionSyntax>();
                foreach (var invocation in invocations)
                {
                    // Console.WriteLine("{0} calls {1}", model.GetDeclaredSymbol(method), model.GetSymbolInfo(invocation).Symbol);

                    var caller = model.GetDeclaredSymbol(method);
                    var callee = model.GetSymbolInfo(invocation).Symbol;

                    string namespaceText = callee.ContainingSymbol.ContainingNamespace.ToString();
                    string classnameText = callee.ContainingSymbol.Name;
                    string methodnameText = callee.Name;

                    Method methodEntity = GetMethodDefinition(namespaceText, classnameText, methodnameText);

                    if (methodEntity != null)
                    {
                        var methodCallEntity = new MethodCall()
                        {
                            Method = methodEntity,
                            CustomFunctionAttribute = attributeEntity,
                            MethodCallArguments = new List<MethodCallArgument>(),
                            CodeStart = invocation.FullSpan.Start,
                            CodeEnd = invocation.FullSpan.End
                        };

                        for (int j = 0; j < invocation.ArgumentList.Arguments.Count; j++)
                        {
                            var arg = new MethodCallArgument()
                            {
                                ArgumentValue = invocation.ArgumentList.Arguments[j].ToFullString(),
                                ArgumentIndex = j + 1,
                                MethodCall = methodCallEntity
                            };
                            methodCallEntity.MethodCallArguments.Add(arg);
                        }

                        if (methodEntity.MethodCalls == null)
                        {
                            methodEntity.MethodCalls = new List<MethodCall>();
                        }

                        if (attributeEntity.MethodCalls == null)
                        {
                            attributeEntity.MethodCalls = new List<MethodCall>();
                        }

                        methodEntity.MethodCalls.Add(methodCallEntity);
                        attributeEntity.MethodCalls.Add(methodCallEntity);
                    } // if (methodEntity != null)


                } // foreach (var invocation in invocations)
            } // foreach (var method in methods)

            DataModel.SaveChanges(); // how slow will this be?            
        }

        private static object syncLock = new Object();

        private Method GetMethodDefinition(string namespaceText, string classnameText, string methodnameText)
        {
            lock(syncLock)
            {

                var methodDefQuery = from m in DataModel.Methods
                                     where m.Namespace == namespaceText
                                     &&
                                     m.ClassName == classnameText
                                     &&
                                     m.MethodName == methodnameText
                                     select m;
                var methodDefs = methodDefQuery.ToList();

                Method methodEntity = null;
                switch (methodDefs.Count)
                {
                    case 0:
                        // create a new method
                        methodEntity = new Method()
                        {
                            Namespace = namespaceText,
                            ClassName = classnameText,
                            MethodName = methodnameText
                        };

                        DataModel.Methods.Add(methodEntity);
                        DataModel.SaveChanges(); // This must happen otherwise we may get duplicates

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\t* Adding new method: {0}.{1}", classnameText, methodnameText);
                        Console.ResetColor();
                        break;

                    case 1:
                        // use existing method
                        methodEntity = methodDefs[0];
                        break;

                    default:
                        throw new Exception("invalid number of method definitions found: " + methodDefs.Count);
                }
                return methodEntity;
            }
            
        }

        private string GetSourceCode(CustomFunction cf)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using System; using Medidata.Core.Objects;");
            sb.Append("using System.Data;");
            sb.Append("using System.Text;");
            sb.Append("using System.Collections;");
            sb.Append("using System.Globalization;");
            sb.Append("using System.Text.RegularExpressions;");
            sb.Append("using Medidata.Core.Common.Utilities;");
            sb.Append("using Medidata.Utilities;");
            sb.Append("using Medidata.Utilities.Interfaces;");
            sb.Append("using Medidata.RaveWebServices.Outbound;");
            sb.Append("class CustomClass:Medidata.Core.Objects.CustomFunctionBase");
            sb.Append("{");
            sb.Append("public CustomClass(){}");
            sb.Append("public override object Eval(object ThisObject)");
            sb.Append("{");
            sb.Append(cf.SourceCode);
            sb.Append("}");
            sb.Append("}");

            return sb.ToString();
        }

        private string GenerateSHA256(string sourceCode)
        {
            SHA256 hashAlgo = SHA256Managed.Create();
            byte[] hashValue = hashAlgo.ComputeHash(new MemoryStream(Encoding.UTF8.GetBytes(sourceCode)));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
                sb.AppendFormat("{0:X2}", hashValue[i]);
            return sb.ToString();
        }

        private CSharpCompilation CreateCompilationUnit(CustomFunction cf, SyntaxTree tree)
        {
            var compilationUnit = CSharpCompilation.Create(String.Format("CustomClass_{0}_{1}", /*cf.DB*/"test", cf.FunctionID)).
                AddReferences(
                      MetadataReference.CreateFromFile(typeof(object).Assembly.Location)
                    , MetadataReference.CreateFromFile(typeof(System.Data.DataSet).Assembly.Location)
                    , MetadataReference.CreateFromFile(typeof(System.Xml.XmlDocument).Assembly.Location)
                    , MetadataReference.CreateFromFile(typeof(System.Text.RegularExpressions.Regex).Assembly.Location)

                    , MetadataReference.CreateFromFile(String.Join("\\", Settings.Default.DefaultBinaryPath, "Medidata.Core.Objects.dll"))
                    , MetadataReference.CreateFromFile(String.Join("\\", Settings.Default.DefaultBinaryPath, "Medidata.Core.Common.dll"))
                    , MetadataReference.CreateFromFile(String.Join("\\", Settings.Default.DefaultBinaryPath, "Medidata.Utilities.dll"))
                    , MetadataReference.CreateFromFile(String.Join("\\", Settings.Default.DefaultBinaryPath, "Medidata.Interfaces.dll"))
                    , MetadataReference.CreateFromFile(String.Join("\\", Settings.Default.DefaultBinaryPath, "Medidata.RaveWebServices.Outbound.dll"))
                ).AddSyntaxTrees(tree);
            return compilationUnit;
        }

        private CustomFunctionDetail CreateDetailRecord(CustomFunction cf, string databaseName)
        {
            var results = new CustomFunctionDetail()
            {
                FunctionID = cf.FunctionID,
                FunctionName = cf.FunctionName,
                CRFVersionID = cf.CRFVersionID,
                Created = cf.Created,
                Updated = cf.Updated,
                OID = cf.OID,
                Lang = cf.Lang,
                SourceCode = cf.SourceCode,
                DB = databaseName
            };

            return results;
        }

        private class MetadataFileReference
        {
            private string location;

            public MetadataFileReference(string location)
            {
                this.location = location;
            }
        }
    }
}
