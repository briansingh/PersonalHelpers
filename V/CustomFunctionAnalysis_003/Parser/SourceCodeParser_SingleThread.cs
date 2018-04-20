using CustomFunctionAnalysis_003.Model;
using CustomFunctionAnalysis_003.Properties;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomFunctionAnalysis_003.Parser
{
    public class SourceCodeParser_SingleThread
    {
        public CustomFunctionsSourceModel SourceDataModel { get; private set; }
        public CustomFunctionsTargetModel TargetDataModel { get; private set; }
        public string DatabaseName { get; private set; }
        public int[] CRFVersions { get; private set; }

        public SourceCodeParser_SingleThread(string databaseName, string sourceConnectionString, int[] crfVersions)
        {
            DatabaseName = databaseName;
            SourceDataModel = new CustomFunctionsSourceModel(sourceConnectionString);
            TargetDataModel = new CustomFunctionsTargetModel();
            CRFVersions = crfVersions;
        }

        public void Run()
        {
            Console.WriteLine("Prcessing Database {0}", DatabaseName);

            IQueryable<CustomFunction> query = from cf in SourceDataModel.CustomFunctions
                                                   //where cf.DB == DatabaseName
                                                   //where cf.OID == "SCS_CF"
                                               where cf.FunctionID == 4589
                                               select cf;

            /*
            if (CRFVersions != null && CRFVersions.Count() > 0)
            {
                query = from cf in query
                        where CRFVersions.Contains(cf.CRFVersionID)
                        select cf;
            }
            */

            foreach (CustomFunction cf in query.ToList())
            {
                Console.WriteLine(" - {0}({1})", cf.FunctionName, cf.FunctionID);


                CustomFunctionDetail detail = CreateDetailRecord(cf, DatabaseName);
                TargetDataModel.CustomFunctionDetails.Add(detail);
                TargetDataModel.SaveChanges();

                string sourceCode = GetSourceCode(cf);
                string sha256 = GenerateSHA256(sourceCode);
                CustomFunctionAttribute attributeEntity = new CustomFunctionAttribute()
                {
                    // CustomFunction = detail,
                    SHA256 = sha256,
                    DB = DatabaseName,
                    FunctionID = cf.FunctionID,
                    MethodCalls = new List<MethodCall>()
                };

                TargetDataModel.CustomFunctionAttributes.Add(attributeEntity);
                TargetDataModel.SaveChanges();

                SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
                CompilationUnitSyntax root = (CompilationUnitSyntax)tree.GetRoot();
                CSharpCompilation compilationUnit = CreateCompilationUnit(cf, tree);
                SemanticModel model = compilationUnit.GetSemanticModel(tree);

                IEnumerable<MethodDeclarationSyntax> methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();
                foreach (MethodDeclarationSyntax method in methods)
                {
                    IEnumerable<InvocationExpressionSyntax> invocations = method.DescendantNodes().OfType<InvocationExpressionSyntax>();
                    foreach (InvocationExpressionSyntax invocation in invocations)
                    {
                        // Console.WriteLine("{0} calls {1}", model.GetDeclaredSymbol(method), model.GetSymbolInfo(invocation).Symbol);

                        IMethodSymbol caller = model.GetDeclaredSymbol(method);
                        SymbolInfo temp = model.GetSymbolInfo(invocation);
                        ISymbol callee = model.GetSymbolInfo(invocation).Symbol;

                        string namespaceText = callee.ContainingSymbol.ContainingNamespace.ToString();
                        string classnameText = callee.ContainingSymbol.Name;
                        string methodnameText = callee.Name;

                        IQueryable<Method> methodDefQuery = from m in TargetDataModel.Methods
                                             where m.Namespace == namespaceText
                                             &&
                                             m.ClassName == classnameText
                                             &&
                                             m.MethodName == methodnameText
                                             select m;
                        List<Method> methodDefs = methodDefQuery.ToList();

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
                                
                                TargetDataModel.Methods.Add(methodEntity);
                                TargetDataModel.SaveChanges(); // This must happen otherwise we may get duplicates

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

                        if (methodEntity != null)
                        {
                            MethodCall methodCallEntity = new MethodCall()
                            {
                                Method = methodEntity,
                                CustomFunctionAttribute = attributeEntity,
                                MethodCallArguments = new List<MethodCallArgument>(),
                                CodeStart = invocation.FullSpan.Start,
                                CodeEnd = invocation.FullSpan.End
                            };

                            for (int j = 0; j < invocation.ArgumentList.Arguments.Count; j++)
                            {
                                MethodCallArgument arg = new MethodCallArgument()
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
                
                TargetDataModel.SaveChanges(); // how slow will this be?

            } // foreach (var cf in query.ToList())
        }

        private string GetSourceCode(CustomFunction cf)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("using Medidata.Cloud.Programmability.CustomFunctions.Customizations;");
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
            CSharpCompilation compilationUnit = CSharpCompilation.Create(String.Format("CustomClass_{0}_{1}", /*cf.DB*/"test", cf.FunctionID)).
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
                    , MetadataReference.CreateFromFile(String.Join("\\", Settings.Default.DefaultBinaryPath, "Medidata.Cloud.Programmability.CustomFunctions.Customizations.dll"))
                ).AddSyntaxTrees(tree);
            return compilationUnit;
        }

        private CustomFunctionDetail CreateDetailRecord(CustomFunction cf, string databaseName)
        {
            CustomFunctionDetail results = new CustomFunctionDetail()
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

    }
}
