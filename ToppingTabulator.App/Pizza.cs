using System.Collections.Generic;
using Newtonsoft.Json;

namespace ToppingTabulator.App
{
    public sealed class Pizza
    {
        [JsonProperty("toppings")]
        public List<string> Toppings { get; set; }
    }
}