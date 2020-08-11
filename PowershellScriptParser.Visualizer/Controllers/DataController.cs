using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PowershellScriptParser.Core;

namespace PowershellScriptParser.Visualizer.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<ForceGraphResult>> Data()
        {
            try
            {
                string filePath = @"example.ps1";
                PowerShellParser powerShellParser = new PowerShellParser();
                var functions = await powerShellParser.GetFunctions(filePath);
                var executions = await powerShellParser.GetFunctionExecutions(filePath, functions);

                var links = executions.Select(x => new ForceGraphResult.ForceGraphLink
                {
                    Source = x.ExecutorName ?? "Main",
                    Target = x.FunctionName,
                    Value = 2
                }).ToList();
                var nodes = links
                    .SelectMany(x => new List<string> { x.Source, x.Target })
                    .Distinct()
                    .Select(x => new ForceGraphResult.ForceGraphNode
                    {
                        Id = x,
                        Group = x.GetHashCode()
                    });

                var forceGraphResult = new ForceGraphResult
                {
                    Nodes = nodes,
                    Links = links
                };
                return Ok(forceGraphResult);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode((int) HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }

    public class ForceGraphResult
    {
        public class ForceGraphNode
        {
            public string Id { get; set; }
            public int Group { get; set; }
        }

        public class ForceGraphLink
        {
            public string Source { get; set; }
            public string Target { get; set; }
            public int Value { get; set; }
        }

        public IEnumerable<ForceGraphLink> Links { get; set; }
        public IEnumerable<ForceGraphNode> Nodes { get; set; }
    }
}