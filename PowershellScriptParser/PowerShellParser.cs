using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PowershellScriptParser
{
    public class PowerShellParser
    {
        public async Task<List<Function>> GetFunctions(string filePath)
        {
            Regex functionNamesRegex =
                new Regex(
                    @"Function\s+(?<functionName>[\w-]+)\s*\(?\s*\)?\s*(?<openingBracket>{)?\s*(?<closingBracket>})?");
            List<Function> functions = new List<Function>();
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string line = await streamReader.ReadLineAsync();
                long lineCount = 1;
                while (line != null)
                {
                    line = line.Trim();
                    Match match = functionNamesRegex.Match(line);
                    if (match.Success)
                    {
                        long numberOfBracketsIn = 0;
                        var newFunction = new Function
                        {
                            StartLine = lineCount,
                            Name = match.Groups["functionName"].Value,
                        };

                        if (newFunction.Name == "Get-HasCompanyName")
                        {
                            int x = 0;
                        }

                        if (match.Groups["openingBracket"].Success && match.Groups["closingBracket"].Success)
                        {
                            newFunction.EndLine = lineCount;
                            functions.Add(newFunction);
                            continue;
                        }

                        if (match.Groups["openingBracket"].Success)
                        {
                            numberOfBracketsIn++;
                        }
                        else
                        {
                            do
                            {
                                line = await streamReader.ReadLineAsync();
                                line = line.Trim();
                                lineCount++;
                            } while (line == string.Empty);

                            if (line.First() == '{')
                            {
                                numberOfBracketsIn++;
                            }
                            else
                            {
                                throw new Exception("Invalid function.");
                            }
                        }

                        while (numberOfBracketsIn != 0)
                        {
                            line = await streamReader.ReadLineAsync();
                            line = line.Trim();
                            lineCount++;
                            numberOfBracketsIn += line.Aggregate(0, (total, c) =>
                            {
                                switch (c)
                                {
                                    case '{':
                                        return total + 1;
                                    case '}':
                                        return total - 1;
                                    default:
                                        return total;
                                }
                            });

                            if (numberOfBracketsIn == 0)
                            {
                                newFunction.EndLine = lineCount;
                                functions.Add(newFunction);
                            }
                        }

                        continue;
                    }

                    line = await streamReader.ReadLineAsync();
                    lineCount++;
                }
            }

            return functions;
        }

        public async Task<List<FunctionExecution>> GetFunctionExecutions(string filePath,
            List<Function> functions)
        {
            var orderedFunctions = functions.OrderByDescending(x => x.Name.Length).ToList();
            var functionExecutions = new List<FunctionExecution>();

            long lineCount = 0;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                var line = await streamReader.ReadLineAsync();
                lineCount++;
                while (line != null)
                {
                    line = line.Trim();

                    if (line == string.Empty || line.StartsWith('#') || line.StartsWith("<#"))
                    {
                        line = await streamReader.ReadLineAsync();
                        lineCount++;
                        continue;
                    }

                    Function currentlyExecutingFunction = orderedFunctions.FirstOrDefault(x => lineCount >= x.StartLine && lineCount <= x.EndLine);
                    foreach (var function in orderedFunctions)
                    {
                        // check if function definition match the function name.
                        if (lineCount != function.StartLine)
                        {
                            // TODO: DEAL WITH PIPES. MULTIPLE FUNCTION CALLS PER LINE.
                            // Not ideal because function names can contain other functions names.
                            if (line.Contains(function.Name) && (!currentlyExecutingFunction?.Name.Contains(function.Name) ?? true))
                            {
                            
                                var executor = currentlyExecutingFunction?.Name;
                                functionExecutions.Add(new FunctionExecution
                                {
                                    ExecutionLine = lineCount,
                                    FunctionName = function.Name,
                                    ExecutorName = executor
                                });
                                break;
                            }
                        }
                    }
                    line = await streamReader.ReadLineAsync();
                    lineCount++;
                }
            }

            return functionExecutions;
        }
    }
}