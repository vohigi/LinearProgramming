using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinearProgrammingProblems.Models
{
    public class Shipment
    {
        public double CostPerUnit { get; set; }
        public int R{ get; set; } 
        public int C{ get; set; }
        public double Quantity { get; set; }

        public Shipment(double q, double cpu, int r, int c)
        {
            Quantity = q;
            CostPerUnit = cpu;
            this.R = r;
            this.C = c;
        }
    }
}
