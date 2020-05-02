using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabReposicion
{
    public class ArbolBinario
    {
        public Nodo Raiz { get; set; }

        public bool esVacio { get { return Raiz == null; } }

        //Constructor 
        public ArbolBinario() { }
    }
}
