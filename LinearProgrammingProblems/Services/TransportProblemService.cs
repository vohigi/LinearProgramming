using System;
using System.Collections.Generic;
using LinearProgrammingProblems.Models;

namespace LinearProgrammingProblems.Services
{
    public class TransportProblemService
    {
        private readonly TransportProblem _transportProblem;
        private Shipment[,] Calculated { get; set; }
        public TransportProblemService(TransportProblem transportProblem)
        {
            _transportProblem = transportProblem;
            Calculated=new Shipment[_transportProblem.Supply.Length,_transportProblem.Demmand.Length];
        }
        /// <summary>
        ///     calculates base cost, base plan, optimized cost, optimized plan
        /// </summary>
        /// <returns>TransportProblemServiceResult</returns>
        public TransportProblemServiceResult Solve()
        {
            TransportProblemServiceResult transportProblemServiceResult = new TransportProblemServiceResult();
            try
            {
                NorthWestCornerRule();
                transportProblemServiceResult.BasePlan = getResultArray(Calculated);
                transportProblemServiceResult.BaseCost = getTotalTransportationCost(Calculated);
                SteppingStone();
                transportProblemServiceResult.OptimizedPlan = getResultArray(Calculated);
                transportProblemServiceResult.OptimizedCost = getTotalTransportationCost(Calculated);
            }
            catch (Exception exception)
            {
                transportProblemServiceResult.Errors=new List<string>(){exception.Message};
                transportProblemServiceResult.Errors.Add(exception.StackTrace);
            }

            return transportProblemServiceResult;
        }
        /// <summary>
        ///     calculates total transportation cost
        /// </summary>
        /// <param name="calculated">2d Shipment array</param>
        /// <returns>double</returns>
        private double getTotalTransportationCost(Shipment[,] calculated)
        {
            double cost = 0;
            for (int r = 0; r < _transportProblem.Supply.Length; r++)
            {
                for (int c = 0; c < _transportProblem.Demmand.Length; c++)
                {
                    Shipment s = calculated[r, c];
                    if (s != null && s.R == r && s.C == c)
                    {
                        cost+=(s.Quantity * s.CostPerUnit);
                    }
                }
            }
            return cost;
        }
        /// <summary>
        ///     converts  2d array of Shipment to 2d double array (saving transportation quantity)
        /// </summary>
        /// <param name="calculated">2d Shipment array</param>
        /// <returns>double[,]</returns>
        private double[,] getResultArray(Shipment[,] calculated)
        {
            double[,] res = new double[_transportProblem.Supply.Length,_transportProblem.Demmand.Length];
            for (int r = 0; r < _transportProblem.Supply.Length; r++)
            {
                for (int c = 0; c < _transportProblem.Demmand.Length; c++)
                {
                    Shipment s = calculated[r, c];
                    if (s != null && s.R == r && s.C == c)
                    {
                        res[r, c] = s.Quantity;
                    }
                    else
                    {
                        res[r, c] = 0;
                    }
                }
            }

            return res;
        }
        /// <summary>
        ///     calculates base plan using north-west corner algorithm
        /// </summary>
        private void NorthWestCornerRule()
        {

            for (int r = 0, northwest = 0; r < _transportProblem.Supply.Length; r++)
                for (int c = northwest; c < _transportProblem.Demmand.Length; c++)
                {

                    int quantity = Math.Min(_transportProblem.Supply[r], _transportProblem.Demmand[c]);
                    if (quantity > 0)
                    {
                        Calculated[r,c] = new Shipment(quantity, _transportProblem.Pricing[r,c], r, c);

                        _transportProblem.Supply[r] -= quantity;
                        _transportProblem.Demmand[c] -= quantity;

                        if (_transportProblem.Supply[r] == 0)
                        {
                            northwest = c;
                            break;
                        }
                    }
                }
        }
        /// <summary>
        ///     Performs base plan optimization using Vogels method
        /// </summary>
        private void SteppingStone() {
        double maxReduction = 0;
        Shipment[] move = null;
        Shipment leaving = null;
 
        fixDegenerateCase();
 
        for (int r = 0; r < _transportProblem.Supply.Length; r++) {
            for (int c = 0; c < _transportProblem.Demmand.Length; c++) {
 
                if (Calculated[r,c] != null)
                    continue;
 
                Shipment trial = new Shipment(0, _transportProblem.Pricing[r,c], r, c);
                Shipment[] path = getClosedPath(trial);
 
                double reduction = 0;
                double lowestQuantity = Int32.MaxValue;
                Shipment leavingCandidate = null;
 
                bool plus = true;
                foreach (var s in path)
                {
                    if (plus) {
                        reduction += s.CostPerUnit;
                    } else {
                        reduction -= s.CostPerUnit;
                        if (s.Quantity < lowestQuantity) {
                            leavingCandidate = s;
                            lowestQuantity = s.Quantity;
                        }
                    }
                    plus = !plus;
                }
                if (reduction < maxReduction) {
                    move = path;
                    leaving = leavingCandidate;
                    maxReduction = reduction;
                }
            }
        }
 
        if (move != null) {
            double q = leaving.Quantity;
            bool plus = true;
            foreach (var s in move) {
                s.Quantity += plus ? q : -q;
                Calculated[s.R,s.C] = s.Quantity == 0 ? null : s;
                plus = !plus;
            }
            SteppingStone();
        }
    }
    /// <summary>
    ///  flat matrix
    /// </summary>
    /// <returns>list of type Shipment</returns>
    private List<Shipment> matrixToList() {
        List<Shipment> res = new List<Shipment>();
        for (int i = 0; i < Calculated.GetLength(0); i++)
        {
            for (int j = 0; j < Calculated.GetLength(1); j++)
            {
                if (Calculated[i,j]!=null)
                {
                    res.Add(Calculated[i, j]);
                }
            }
        }

        return res;
    }
     /// <summary>
     ///     returns plus minus cycle elements
     /// </summary>
     /// <param name="s">added to basis cell</param>
     /// <returns>array of <Shipment></returns>
    private Shipment[] getClosedPath(Shipment s) {
        List<Shipment> path = matrixToList();
        
        // remove (and keep removing) elements that do not have a
        // vertical AND horizontal neighbor
        path.Insert(0,s);
        while (path.RemoveAll(sh =>
        {
            Shipment[] nbrs = getNeighbors(sh, path);
            Console.WriteLine("ne");
            return (nbrs[0] == null) || (nbrs[1] == null);
        })>0);
        
        // place the remaining elements in the correct plus-minus order
        Shipment[] stones = path.ToArray();
        Shipment prev = s;
        for (int i = 0; i < stones.Length; i++) {
            stones[i] = prev;
            prev = getNeighbors(prev, path)[i % 2];
        }
        return stones;
    }
    /// <summary>
    ///     returns array of vertical and horizontal neighbours for the cell
    /// </summary>
    /// <param name="s">cell to get neighbours for</param>
    /// <param name="lst">list of all cells</param>
    /// <returns>array of type Shipment with size 2</returns>
    private Shipment[] getNeighbors(Shipment s, List<Shipment> lst) {
        Shipment[] nbrs = new Shipment[2];
        foreach (var o in lst) {
            if (o == s) continue;
            if (o.R == s.R && nbrs[0] == null)
                nbrs[0] = o;
            else if (o.C == s.C && nbrs[1] == null)
                nbrs[1] = o;
            if (nbrs[0] != null && nbrs[1] != null)
                break;
        }
        return nbrs;
    }
    /// <summary>
    ///     fix degenerate case by adding dummy values
    /// </summary>
    private void fixDegenerateCase() {
        double eps = 4.9E-324;
 
        if (_transportProblem.Supply.Length + _transportProblem.Demmand.Length - 1 != matrixToList().Count) {
 
            for (int r = 0; r < _transportProblem.Supply.Length; r++)
                for (int c = 0; c < _transportProblem.Demmand.Length; c++) {
                    if (Calculated[r,c] == null) {
                        Shipment dummy = new Shipment(eps, _transportProblem.Pricing[r,c], r, c);
                        if (getClosedPath(dummy).Length == 0) {
                            Calculated[r,c] = dummy;
                            return;
                        }
                    }
                }
        }
    }

        
        
    }
}