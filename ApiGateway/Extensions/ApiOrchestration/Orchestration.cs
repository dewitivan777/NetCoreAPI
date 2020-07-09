﻿using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace ApiGateway.ApiGateway
{
    public class Orchestration
    {
        public string Api { get; set; }

        public IEnumerable<Route> Routes { get; set; }
    }

    public class Route
    {
        public string Key { get; set; }

        public string Verb { get; set; }

        public JsonSchema RequestJsonSchema { get; set; }

        public JsonSchema ResponseJsonSchema { get; set; }
    }
}
