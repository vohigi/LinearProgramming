using System;

namespace LinearProgrammingProblems.Models
{
  public class TransportProblem{
    public int[] Supply { get; set; }
    public int[] Demmand{ get; set; }
    public double[,] Pricing{ get; set; }

  }
}