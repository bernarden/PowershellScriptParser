using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Visualizer.Controllers
{
    [Route("api/[controller]")]
    public class DataController : Controller
    {
        [HttpGet]
        public ActionResult<ForceGraphResult> Data()
        {
            return Ok(new ForceGraphResult
            {
                Nodes = new List<ForceGraphResult.ForceGraphNode>
                {
                    new ForceGraphResult.ForceGraphNode
                    {
                        Group = 1,
                        Id = "A"
                    },
                    new ForceGraphResult.ForceGraphNode
                    {
                        Group = 2,
                        Id = "B"
                    }
                },
                Links = new List<ForceGraphResult.ForceGraphLink>
                {
                    new ForceGraphResult.ForceGraphLink()
                    {
                        Source = "A",
                        Target = "B",
                        Value = 2
                    }
                }
            });
        }
    }

    public class ForceGraphResult
    {
        public class ForceGraphNode
        {
            public string Id { get; set; }
            public int Group { get;set; }
        }

        public class ForceGraphLink
        {
            public string Source { get; set; }
            public string Target { get; set; }
            public int Value { get; set; }
        }

        public IEnumerable<ForceGraphLink> Links { get;set; }
        public IEnumerable<ForceGraphNode> Nodes { get;set; }
    }
}
