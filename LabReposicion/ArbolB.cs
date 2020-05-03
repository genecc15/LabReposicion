using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabReposicion
{
    public class ArbolB
    {
        public void Insertar(Bebida Nuevo)
        {
            if (!Singleton.Instance.Diccionario.ContainsKey(Nuevo.Nombre))
            {
                Singleton.Instance.MostrarArbol.Add(Nuevo);
                Singleton.Instance.Diccionario.Add(Nuevo.Nombre, Nuevo);

                if (Singleton.Instance.PrimeraSeparacion && !Singleton.Instance.TercerNivel)
                {//2 niveles
                    EncontrarEspacio(ref Singleton.Instance.Raiz, Nuevo, ref Singleton.Instance.Raiz);

                }
                else if (Singleton.Instance.PrimeraSeparacion && Singleton.Instance.TercerNivel)
                { // 3 niveles
                    Navegar(Nuevo, Singleton.Instance.Raiz);
                }
                else
                {
                    var contador = 0;
                    foreach (var item in Singleton.Instance.Raiz.Datos)
                    {
                        if (item == null)
                        {
                            Singleton.Instance.Raiz.Datos[contador] = Nuevo;
                            SortDatos(ref Singleton.Instance.Raiz.Datos);
                            break;
                        }
                        contador++;
                        if (contador == Singleton.Instance.Raiz.Datos.Length)
                        {
                            // está lleno
                            PrimeraSeparacion(Singleton.Instance.Raiz, Nuevo);
                            break;
                            //partir
                        }
                    }
                }
            }

        }
        public void EncontrarEspacio(ref NodoB Raiz, Bebida Nuevo, ref NodoB Actual)
        {
            var IndiceCorrecto = EncontrarHoja(Nuevo);

            if (Actual.Hijos[IndiceCorrecto] == null)
            {
                // es hoja
                Singleton.Instance.IndiceHijoActual = IndiceCorrecto;
                InsertarEnHoja(ref Actual, Nuevo, ref Raiz);
            }
            else
            {
                // no es hoja
                Singleton.Instance.IndiceHijoActual = IndiceCorrecto;
                EncontrarEspacio(ref Raiz, Nuevo, ref Actual.Hijos[IndiceCorrecto]);

            }

        }
        public void InsertarEnHoja(ref NodoB Hoja, Bebida Nuevo, ref NodoB Padre)
        {
            // No encontro un espacio, hay que partir
            if (EstaLleno(Hoja) && Padre != null && EstaLleno(Hoja.Padre))
            {

                Segunda(ref Nuevo);
                return;

            }

            bool insertado = false;
            var indice = 0;
            //si no se inserta, seguira buscando hasta encontrar o que haya que partir
            while (!insertado)
            {
                if (indice == (Hoja.Grado - 1))
                {

                    PartirHijo(ref Hoja, ref Hoja.Padre, Nuevo);
                    insertado = true;
                }
                else
                {
                    // encontro un espacio deisponible, no Paarte
                    if (Hoja.Datos[indice] == null)
                    {
                        Hoja.Padre = Padre;
                        Hoja.Datos[indice] = Nuevo;
                        SortDatos(ref Hoja.Datos);
                        insertado = true;
                    }
                    else
                    {
                        indice++;

                    }
                }
            }
        }
        public void Segunda(ref Bebida Nuevo)
        {

            #region Partiedo Hijos
            var ArregloHijosAAsignar = new NodoB[Singleton.Instance.Raiz.Grado + 1];
            var ArregloDatosHijoQueSePArte = new Bebida[Singleton.Instance.Raiz.Grado];

            var hijosapartir = Singleton.Instance.Raiz.Hijos[Singleton.Instance.IndiceHijoActual];
            var HijoPardidoIzquierda = new NodoB(Singleton.Instance.Raiz.Grado);
            var HijoPardidoDerecha = new NodoB(Singleton.Instance.Raiz.Grado);
            var contador = 0;

            for (int i = 0; i < Singleton.Instance.IndiceHijoActual; i++)
            {
                ArregloHijosAAsignar[i] = Singleton.Instance.Raiz.Hijos[i];
            }
            for (int i = Singleton.Instance.IndiceHijoActual + 1; i < Singleton.Instance.Raiz.Grado; i++)
            {
                ArregloHijosAAsignar[i + 1] = Singleton.Instance.Raiz.Hijos[i];

            }
            //Llenando hijo izquierdo
            contador = 0;
            foreach (var item in hijosapartir.Datos)
            {
                ArregloDatosHijoQueSePArte[contador] = hijosapartir.Datos[contador];
                contador++;
            }
            ArregloDatosHijoQueSePArte[contador] = Nuevo;
            SortDatos(ref ArregloDatosHijoQueSePArte);
            contador = 0;
            for (int i = 0; i < ArregloDatosHijoQueSePArte.Length / 2; i++)
            {
                HijoPardidoIzquierda.Datos[contador] = ArregloDatosHijoQueSePArte[i];
                contador++;
            }
            contador = 0;
            for (int i = (ArregloDatosHijoQueSePArte.Length / 2) + 1; i < ArregloDatosHijoQueSePArte.Length; i++)
            {
                HijoPardidoDerecha.Datos[contador] = ArregloDatosHijoQueSePArte[i];
                contador++;
            }

            contador = 0;
            foreach (var item in ArregloHijosAAsignar)
            {
                if (item == null)
                {
                    ArregloHijosAAsignar[contador] = HijoPardidoIzquierda; contador++;
                    break;
                }
                contador++;
            }

            //Asginando hijo derecho
            ArregloHijosAAsignar[contador] = HijoPardidoDerecha;


            #endregion
            //partir Raiz
            #region Partiendo Raiz Actual

            var ArregloRai_A_Asignar = new NodoB[Singleton.Instance.Raiz.Grado + 1];
            var ArregloDatosPadreQueSePArte = new Bebida[Singleton.Instance.Raiz.Grado];
            var PadreIzquierdo = new NodoB(Singleton.Instance.Raiz.Grado);
            var PadreDerecho = new NodoB(Singleton.Instance.Raiz.Grado);
            var arregloRaiz = new Bebida[Singleton.Instance.Raiz.Grado];
            contador = 0;
            foreach (var item in Singleton.Instance.Raiz.Datos)
            {
                arregloRaiz[contador] = Singleton.Instance.Raiz.Datos[contador];
                contador++;
            }
            arregloRaiz[contador] = ArregloDatosHijoQueSePArte[ArregloDatosHijoQueSePArte.Length / 2];

            SortDatos(ref arregloRaiz);
            contador = 0;
            for (int i = 0; i < arregloRaiz.Length / 2; i++)
            {
                PadreIzquierdo.Datos[contador] = arregloRaiz[i];
                contador++;
            }
            contador = 0;
            for (int i = (arregloRaiz.Length / 2) + 1; i < arregloRaiz.Length; i++)
            {
                PadreDerecho.Datos[contador] = arregloRaiz[i];
                contador++;
            }
            var NuevaRaiz = new NodoB(Singleton.Instance.Raiz.Grado);
            NuevaRaiz.Datos[0] = arregloRaiz[arregloRaiz.Length / 2];
            PadreDerecho.Padre = NuevaRaiz;
            PadreIzquierdo.Padre = NuevaRaiz;
            // Subir mitad de raiz
            NuevaRaiz.Datos[0] = arregloRaiz[arregloRaiz.Length / 2];
            PadreIzquierdo.Padre = NuevaRaiz;
            PadreDerecho.Padre = NuevaRaiz;
            contador = 0;
            for (int i = 0; i < ArregloHijosAAsignar.Length / 2; i++)
            {
                PadreIzquierdo.Hijos[i] = ArregloHijosAAsignar[contador];
                ArregloHijosAAsignar[contador].Padre = PadreIzquierdo;
                contador++;
            }
            for (int i = 0; i < ArregloHijosAAsignar.Length / 2; i++)
            {
                PadreDerecho.Hijos[i] = ArregloHijosAAsignar[contador];
                ArregloHijosAAsignar[contador].Padre = PadreDerecho;
                contador++;

            }
            #endregion
            //asignar padres
            NuevaRaiz.Hijos[0] = PadreIzquierdo;
            NuevaRaiz.Hijos[1] = PadreDerecho;
            Singleton.Instance.Raiz = NuevaRaiz;
            Singleton.Instance.TercerNivel = true;
        }
        public void PrimeraSeparacion(NodoB Actual, Bebida Nuevo)
        {
            var AuxPartir = new Bebida[Actual.Grado];
            var contador = 0;
            foreach (var item in Actual.Datos)
            {
                AuxPartir[contador] = item;
                contador++;
            }
            AuxPartir[contador] = Nuevo;
            SortDatos(ref AuxPartir);
            Actual = new NodoB(Actual.Grado);
            //asignamos el del centro al nuevo padre
            Actual.Datos[0] = AuxPartir[(Actual.Grado / 2)];
            for (int i = 0; i < Actual.Grado; i++)
            {
                Actual.Hijos[i] = new NodoB(Actual.Grado);
            }
            //asignamos los izquierdos
            contador = 0;
            for (int i = 0; i < AuxPartir.Length / 2; i++)
            {
                Actual.Hijos[0].Datos[contador] = AuxPartir[i];
                contador++;
            }
            //asignamos los Derechos

            contador = 0;
            for (int i = (AuxPartir.Length / 2) + 1; i < AuxPartir.Length; i++)
            {
                Actual.Hijos[1].Datos[contador] = AuxPartir[i];
                contador++;
            }

            Singleton.Instance.Raiz = Actual;
            Singleton.Instance.PrimeraSeparacion = true;
        }
        public bool EstaLleno(NodoB Actual)
        {
            for (int i = 0; i < Actual.Grado - 1; i++)
            {
                if (Actual.Datos[i] == null)
                {
                    return false;
                }
            }
            SortDatos(ref Actual.Datos);
            return true;
        }
        public void PartirHijo(ref NodoB Actual, ref NodoB Padre, Bebida Nuevo)
        {
            var conta = 0;
            foreach (var item in Padre.Datos)
            {
                //No encuentra lugar en el padre, entonces se parte la raiz
                if (conta == Padre.Datos.Length)
                {

                }
            }


            var ArregloOrdenado = new Bebida[Actual.Grado];
            for (int i = 0; i < Actual.Datos.Length; i++)
            {
                ArregloOrdenado[i] = Actual.Datos[i];
            }
            ArregloOrdenado[ArregloOrdenado.Length - 1] = Nuevo;
            SortDatos(ref ArregloOrdenado);
            var Sube = ArregloOrdenado[(ArregloOrdenado.Length / 2)];
            //asignamos padre a los nuevos
            var Ladoizquierdo = new NodoB(Padre.Grado)
            {
                Padre = Padre
            };
            var LadoDerecho = new NodoB(Padre.Grado)
            {
                Padre = Padre
            };
            var indice = 0;
            for (int i = 0; i < (ArregloOrdenado.Length / 2); i++)
            {
                Ladoizquierdo.Datos[i] = ArregloOrdenado[indice];
                indice++;
            }
            indice++;
            for (int i = 0; i < (ArregloOrdenado.Length / 2); i++)
            {
                LadoDerecho.Datos[i] = ArregloOrdenado[indice];
                indice++;
            }
            //---------------------------------PROBANDO---------------------------
            var auxHijosParaMover = new NodoB[Padre.Grado];
            var indice_asignar = 0;
            //desde el primero hasta el que toca partir
            for (int i = 0; i < Singleton.Instance.IndiceHijoActual; i++)
            {
                auxHijosParaMover[i] = Padre.Hijos[i];
                indice_asignar = i;
            }
            //asignar los que se van a partir
            if (Singleton.Instance.IndiceHijoActual > 0)
            {
                indice_asignar++;
            }

            auxHijosParaMover[indice_asignar] = Ladoizquierdo;
            indice_asignar++;
            auxHijosParaMover[indice_asignar] = LadoDerecho;
            indice_asignar++;

            //asignar a partir de los nuevos a los derechos sobrantes
            for (int i = indice_asignar; i < Padre.Hijos.Length; i++)
            {
                auxHijosParaMover[i] = Padre.Hijos[i - 1];
            }

            Padre.Hijos = auxHijosParaMover;

            //reordenar Raiz
            var auxpadre = new List<Bebida>();
            foreach (var item in Padre.Datos)
            {
                if (item != null)
                {
                    auxpadre.Add(item);
                }
            }
            auxpadre.Add(Sube);
            auxpadre = auxpadre.OrderBy(o => o.Nombre).ToList();
            var y = 0;
            //sube dato al padre
            foreach (var item in auxpadre)
            {
                //No encuentra lugar en el padre, entonces se parte la raiz
                Padre.Datos[y] = item;
                y++;
            }
        }
        public int EncontrarHoja(Bebida Nuevo)
        {
            // va al maz izquierdo
            if ((string.Compare(Nuevo.Nombre, Singleton.Instance.Raiz.Datos[0].Nombre)) == -1)
            {
                return 0;
            }

            // va al mas derecho de los hijos
            var listacontar = new List<Bebida>();
            foreach (var item in Singleton.Instance.Raiz.Datos)
            {
                if (item != null)
                {
                    listacontar.Add(item);
                }
            }
            var totalelemtentoslista = listacontar.Count;


            if ((string.Compare(Nuevo.Nombre, Singleton.Instance.Raiz.Datos[totalelemtentoslista - 1].Nombre)) == 1)
            {
                return (totalelemtentoslista);
            }
            // encontrar hijo del medio

            for (int i = 0; i < Singleton.Instance.Raiz.Datos.Length - 1; i++)
            {
                var izquierdo = string.Compare(Nuevo.Nombre, Singleton.Instance.Raiz.Datos[i].Nombre);
                var derecho = string.Compare(Nuevo.Nombre, Singleton.Instance.Raiz.Datos[i].Nombre);

                if (izquierdo != 1)
                {
                    return i;
                }
                else if (derecho != 1)
                {
                    return i + 1;
                }
            }
            return 99;
        }
        public void SortDatos(ref Bebida[] A_Arreglar)
        {
            var lista = new List<Bebida>();
            foreach (var item in A_Arreglar)
            {
                if (item != null)
                {
                    lista.Add(item);
                }
            }
            lista = lista.OrderBy(o => o.Nombre).ToList();
            var contador = 0;
            foreach (var item in lista)
            {
                A_Arreglar[contador] = item;
                contador++;
            }
        }
        public Bebida RetornaBuscar(string Nombre, ref NodoB Actual)
        {
            var ABuscar = new Bebida
            {
                Nombre = Nombre
            };
            //busca en el actual
            for (int i = 0; i < Actual.Grado - 1; i++)
            {
                //no validara si son null
                if (Actual.Datos[i] != null && Actual.Datos[i].Nombre == Nombre)
                {
                    return Actual.Datos[i];
                }
            }
            //si no esta, va a la hoja correspondiente

            //var Indice = EncontrarHoja(ABuscar);
            return null;
        }
        public bool EsHoja(ref NodoB Actual)
        {
            bool resultado = false;
            for (int i = 0; i < Actual.Grado; i++)
            {
                if (Actual.Hijos[i] != null)
                {
                    resultado = true;
                    break;
                }
                else
                {
                    resultado = true;

                }
            }
            return resultado;
        }
        public int IndiceHijo(Bebida Nuevo, NodoB Actual)
        {
            var ListaConteo = new List<Bebida>();
            //Compara si no es la misma posicion

            if ((string.Compare(Nuevo.Nombre, Actual.Datos[0].Nombre)) == 0)
            {
                return 0;
            }

            foreach (var item in Actual.Datos)
            {
                if (item != null)
                {
                    ListaConteo.Add(item);
                }
            }
            var TotEle = ListaConteo.Count;

            if ((string.Compare(Nuevo.Nombre, Actual.Datos[TotEle - 1].Nombre)) == 1)
            {
                return (TotEle);
            }

            for (int i = 0; i < Actual.Datos.Length - 1; i++)
            {
                var izq = string.Compare(Nuevo.Nombre, Actual.Datos[i].Nombre);
                var der = string.Compare(Nuevo.Nombre, Actual.Datos[i].Nombre);
                if (izq != 1)
                {
                    return i;
                }
                else if (der != 1)
                {
                    return i + 1;
                }
            }
            return 99;
        }
        public int Indice(NodoB Actual, Bebida Nuevo)
        {
            var iActualndice = 0;
            var listacomparar = new List<Bebida>();
            foreach (var item in Actual.Datos)
            {
                if (item != null)
                {
                    listacomparar.Add(item);
                }
            }


            for (iActualndice = 0; iActualndice <= listacomparar.Count; iActualndice++)
            {
                if (iActualndice == listacomparar.Count)
                {

                    break;

                }
                else if (String.Compare(Nuevo.Nombre, listacomparar[iActualndice].Nombre) == -1)
                {

                    break;
                }
            }


            return iActualndice;
        }
        public void Navegar(Bebida Nuevo, NodoB Actual)
        {
            var indice = Indice(Actual, Nuevo);
            if (Actual.Hijos[indice] != null)
            {
                Navegar(Nuevo, Actual.Hijos[indice]);

            }
            else
            {
                //insertar
                InsertarEnHoja(ref Actual, Nuevo, ref Actual.Padre);
            }


        }
        public string Buscar(Bebida Nuevo, NodoB Actual)
        {
            // bool encontrado = false;
            for (int i = 0; i < Actual.Datos.Length; i++)
            {
                if (Actual.Datos[i] == null)
                {
                    break;
                }
                else
                {
                    var indice = Indice(Actual, Nuevo);
                    if (Actual.Hijos[indice] != null)
                    {
                        var nn = Nuevo.ToString();
                        var nodoact = Actual.Hijos[indice];
                        var note = RetornaBuscar(nn, ref nodoact);
                        var notee = note.ToString();
                        return notee;
                    }
                }
            }
            return null;
        }
    }
}
