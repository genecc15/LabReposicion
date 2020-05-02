using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabReposicion
{
    public class Nodo
    {
        public byte Fact { get; set; }
        public decimal Probability { get; set; }
        public Nodo Parent { get; set; }
        public Nodo LeftNode { get; set; }
        public Nodo RightNode { get; set; }

        public Nodo(byte _fact, decimal _probability)
        {
            Fact = _fact;
            Probability = _probability;
            LeftNode = null;
            RightNode = null;
            Parent = null;
        }

        public Nodo(decimal _probability)
        {
            Probability = _probability;
            LeftNode = null;
            RightNode = null;
            Parent = null;
        }
        public bool IsLeaf()
        {
            return (RightNode == null && LeftNode == null) ? true : false;
        }
    }
}
