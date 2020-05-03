using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabReposicion
{
    public class Singleton
    {
        private static Singleton _instance = null;
        public static Singleton Instance
        {
            get
            {
                if (_instance == null) _instance = new Singleton();
                return _instance;
            }
        }
        public bool PrimeraSeparacion = false;
        public bool TercerNivel = false;
        public NodoB Raiz = new NodoB(7);
        public int IndiceHijoActual;
        public List<Bebida> MostrarArbol = new List<Bebida>();
        public Dictionary<string, Bebida> Diccionario = new Dictionary<string, Bebida>();

        public Bebida Encontrado = new Bebida();
    }
}
