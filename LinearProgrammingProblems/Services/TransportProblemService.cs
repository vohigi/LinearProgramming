using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
 
    private List<Shipment> matrixToList() {
        //LinkedList<Shipment> res = new LinkedList<Shipment>();
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
//        return stream(matrix)
//                .flatMap(row -> stream(row))
//                .filter(s -> s != null)
//                .collect(toCollection(LinkedList::new));
    }
 
    private Shipment[] getClosedPath(Shipment s) {
        List<Shipment> path = matrixToList();
//        LinkedList<Shipment> path = matrixToList();
        path.Insert(0,s);
        while (path.RemoveAll(sh =>
        {
            Shipment[] nbrs = getNeighbors(sh, path);
            Console.WriteLine("ne");
            return (nbrs[0] == null) || (nbrs[1] == null);
        })>0);
        // remove (and keep removing) elements that do not have a
        // vertical AND horizontal neighbor
//        for (LinkedListNode<Shipment> n2 = path.First; n2 != null; )
//        {
//            LinkedListNode<Shipment> temp = n2.Next;
//            Shipment[] nbrs = getNeighbors(n2.Value, path);
//            if (nbrs[0] == null || nbrs[1] == null)
//                path.Remove(n2);
//            n2 = temp;
//        }
//        var node = path.First;
//        while (node != null)
//        {
//            var next = node.Next;
//            Shipment[] nbrs = getNeighbors(node.Value, path);
//            if (nbrs[0] == null || nbrs[1] == null)
//                path.Remove(node);
//            node = next;
//        }

        // place the remaining elements in the correct plus-minus order
        
        Shipment[] stones = path.ToArray();
        Shipment prev = s;
        for (int i = 0; i < stones.Length; i++) {
            stones[i] = prev;
            prev = getNeighbors(prev, path)[i % 2];
        }
        return stones;
    }
 
    private Shipment[] getNeighbors(Shipment s, List<Shipment> lst) {
        Shipment[] nbrs = new Shipment[2];
        foreach (var o in lst) {
            if (o != s) {
                if (o.R == s.R && nbrs[0] == null)
                    nbrs[0] = o;
                else if (o.C == s.C && nbrs[1] == null)
                    nbrs[1] = o;
                if (nbrs[0] != null && nbrs[1] != null)
                    break;
            }
        }
        return nbrs;
    }
 
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