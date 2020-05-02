using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabReposicion
{
    public class Nodo
    {
        //Propiedades
        public char? letra { get; set; }
        public int frecuencia { get; set; }

        public Nodo padre { get; set; }
        public Nodo izquierdo { get; set; }
        public Nodo derecho { get; set; }

        //Constructor 
        public Nodo()
        {
            this.letra = null;
            this.frecuencia = 0;
        }
        public Nodo(char? letra, int frecuencia)
        {
            this.letra = letra;
            this.frecuencia = frecuencia;
        }
        public Nodo(char? letra, int frecuencia, Nodo padre)
        {
            this.letra = letra;
            this.frecuencia = frecuencia;
            this.padre = padre;
        }

        //Metodos
        public char getChar()
        {
            if (letra != null)
            {
                return this.letra.ToString().ToCharArray()[0];
            }
            else
            {
                return ' ';
            }
        }

        public bool esRaiz()
        {
            if (padre != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool esHoja()
        {
            if ((derecho == null) && (izquierdo == null))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool esNodoIntermedio()
        {
            if (!esHoja() && !esRaiz())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool existeIzquierdo()
        {
            if (izquierdo != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool existeDerecho()
        {
            if (derecho != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
