using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabReposicion
{
    public class ArbolHuffman: ArbolBinario
    {
        //Constructor

        public ArbolHuffman(Dictionary<char, int> lista)
        {
            this.crearArbol(lista);
        }

        //Operations

        public void crearArbol(Dictionary<char, int> lista)
        {

            this.Raiz = null;

            List<Nodo> nodos = new List<Nodo>();
            convertirDiccionarioALista(lista, ref nodos);

            while (nodos.Count > 1) //verificar que solo haya un elemento para finalizar
            {
                //Ingresar valores de la lista en el arbol

                nodos = nodos.OrderBy(o => o.frecuencia).ToList(); //Ordenar lista

                Nodo nodo1 = nodos[0];
                Nodo nodo2 = nodos[1];

                Nodo resultante = fusionarNodos(nodo1, nodo2);

                nodos.Remove(nodo1);
                nodos.Remove(nodo2);

                nodos.Add(resultante);
            }

            //Convertir al elemento faltante de el lista en la raiz
            Raiz = nodos[0];

        }

        private void convertirDiccionarioALista(Dictionary<char, int> lista, ref List<Nodo> listaNodos)
        {
            //Toma los valores y frecuencias del diccionario y los ingresa en un nodo que posteriormente se agrega al arbol

            foreach (var item in lista)
            {
                Nodo tmp = new Nodo();

                tmp.frecuencia = item.Value;
                tmp.letra = item.Key;

                listaNodos.Add(tmp);
            }
        }

        private Nodo fusionarNodos(Nodo nodo1, Nodo nodo2)
        {
            //Construccion de arbol binario desde las hojas hasta la raiz

            int frec = nodo1.frecuencia + nodo2.frecuencia;

            Nodo padre = new Nodo(null, frec);

            padre.izquierdo = nodo1;
            padre.derecho = nodo2;

            return padre;
        }

        public Dictionary<char, string> obtenerDiccionarioBinario()
        {
            // Devueleve un diccionario con pares de caracter y binario

            Dictionary<char, string> diccionario = new Dictionary<char, string>();
            string binario = "";

            //Llamada a recorrer el arbol binario
            recorridoArbol(this.Raiz, ref diccionario, binario);

            return diccionario;
        }

        private void recorridoArbol(Nodo nodo, ref Dictionary<char, string> diccionario, string binario)
        {
            //Agrega un 0 si se recorre hacia la izquierda y un 1 si se va hacia la derecha hasta llegar a la hoja que contiene el simbolo

            //Se toma la misma cadena de binario recorrida hasta ese momento para poseteriormente agregar los valore 1 o 0 al final
            string listaIzquierda = binario;
            string listaDerecha = binario;

            if (!nodo.esHoja())
            {

                //Recorrer nodos a la izquierda
                //listaIzquierda.Add(false);
                listaIzquierda += "0";
                recorridoArbol(nodo.izquierdo, ref diccionario, listaIzquierda);

                //Recorrer nodos a la derecha
                //listaDerecha.Add(true);
                listaDerecha += "1";
                recorridoArbol(nodo.derecho, ref diccionario, listaDerecha);

            }
            else
            {
                //Si el nodo es hoja se agrega al diccionario su recorrido
                diccionario.Add(nodo.getChar(), binario);
            }
        }
    }
}
