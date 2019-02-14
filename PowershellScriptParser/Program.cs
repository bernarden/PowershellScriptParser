using System;
using System.Threading.Tasks;

namespace PowershellScriptParser
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var filePath = "";

            PowerShellParser powerShellParser = new PowerShellParser();
            var functions = await powerShellParser.GetFunctions(filePath);
            foreach (var function in functions)
            {
                Console.WriteLine($"{function.StartLine}-{function.EndLine}  {function.Name}");
            }

            var executions = await powerShellParser.GetFunctionExecutions(filePath, functions);
            foreach (var execution in executions)
            {
                Console.WriteLine($"{execution.ExecutorName ?? "NA"}:{execution.ExecutionLine} -> {execution.FunctionName}");
            }
        }
    }
}