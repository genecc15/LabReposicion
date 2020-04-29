using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace LabReposicion.Cifrados
{
    public class ZigZagMetodos
    {
        public static string CurrentFile = "";
        public static void ZigZagAlgortimo(string Rpath, string Wpath, int llave)
        {
            Cifrado(Rpath, Wpath, llave);
        }

        public static void ZigZagAlgortimo2(string Rpath, string Wpath, int llave)
        {
            Descifrar(Rpath, Wpath, llave);
        }
        #region Cifrado
        public static void Cifrado(string Rpath, string Wpath, int Corrimiento)
        {
            string Data = System.IO.File.ReadAllText(Rpath, Encoding.Default);
            string mensaje = Data;
            var lineas = new List<StringBuilder>();
            for (int i = 0; i < Corrimiento; i++)
            {
                lineas.Add(new StringBuilder());
            }
            int ActualL = 0;
            int Direccion = 1;
            //For para saber donde empezamos

            for (int i = 0; i < mensaje.Length; i++)
            {
                lineas[ActualL].Append(mensaje[i]);

                if (ActualL == 0)
                    Direccion = 1;
                else if (ActualL == Corrimiento - 1)
                    Direccion = -1;

                ActualL += Direccion;
            }
            StringBuilder CifradoFinal = new StringBuilder();

            //Saber donde se encuentra cada caracter
            for (int i = 0; i < Corrimiento; i++)
                CifradoFinal.Append(lineas[i].ToString());

            string Cifrados = CifradoFinal.ToString();

            File.WriteAllText(Wpath, Cifrados);
            CurrentFile = Wpath;
        }
        #endregion
        #region Descifrado
        public static void Descifrar(string Rpath, string Wpath, int corrimiento)
        {
            string Data = System.IO.File.ReadAllText(Rpath, Encoding.Default);
            string mensaje = Data;
            var lineas = new List<StringBuilder>();
            int niveles = corrimiento;


            for (int i = 0; i < corrimiento; i++)
            {
                lineas.Add(new StringBuilder());
            }

            int[] LineaI = Enumerable.Repeat(0, corrimiento).ToArray();

            int ActualL = 0;
            int Direccion = 1;

            //Donde inicia
            for (int i = 0; i < mensaje.Length; i++)
            {
                LineaI[ActualL]++;

                if (ActualL == 0)
                    Direccion = 1;
                else if (ActualL == corrimiento - 1)
                    Direccion = -1;

                ActualL += Direccion;
            }

            int ActualPosicion = 0;

            for (int j = 0; j < corrimiento; j++)
            {
                for (int c = 0; c < LineaI[j]; c++)
                {
                    lineas[j].Append(mensaje[ActualPosicion]);
                    ActualPosicion++;
                }
            }

            StringBuilder descifrado = new StringBuilder();

            ActualL = 0;
            Direccion = 1;

            int[] LP = Enumerable.Repeat(0, corrimiento).ToArray();

            //Une el nuevo orden de las letras
            for (int i = 0; i < mensaje.Length; i++)
            {
                descifrado.Append(lineas[ActualL][LP[ActualL]]);
                LP[ActualL]++;

                if (ActualL == 0)
                    Direccion = 1;
                else if (ActualL == corrimiento - 1)
                    Direccion = -1;

                ActualL += Direccion;
            }

            string DescifradoF = descifrado.ToString();

            File.WriteAllText(Wpath, DescifradoF);
            CurrentFile = Wpath;

        }
        #endregion
    }
}
