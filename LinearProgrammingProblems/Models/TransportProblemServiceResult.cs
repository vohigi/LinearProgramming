using System.Collections.Generic;
using Newtonsoft.Json;

namespace LinearProgrammingProblems.Models
{
    public class TransportProblemServiceResult
    {
        [JsonProperty("base_plan")]
        public double[,] BasePlan { get; set; }
        [JsonProperty("base_cost")]
        public double BaseCost { get; set; }
        [JsonProperty("optimized_plan")]
        public double[,] OptimizedPlan { get; set; }
        [JsonProperty("optimized_cost")]
        public double OptimizedCost { get; set; }
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}