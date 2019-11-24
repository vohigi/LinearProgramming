using Newtonsoft.Json;

namespace LinearProgrammingProblems.Models
{
    public class AssignmentProblemServiceResult
    {
        [JsonProperty("finalMatrix")]
        public int [,] FinalMatrix { get; set; }
        [JsonProperty("solutionValues")]
        public int [] SolutionValues { get; set; }
        [JsonProperty("functionValue")]
        public int FunctionValue { get; set; }
    }
}