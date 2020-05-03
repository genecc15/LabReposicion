using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Numerics;

namespace LabReposicion
{
    public class RSA
    {
        public static string PathW { get; set; }
        public static string GenerarLlaves(int P, int Q)
        {

            #region variables
            //Declaracion de variables
            int n = P * Q;
            int phi = ((P - 1) * (Q - 1));
            int phi2 = phi;
            int phi3 = phi;
            int phi4 = phi;
            int a;
            int contador = 0;
            int d = 1;
            int e = NumerosPrimos.obtenerNumeroE(n, phi);
            int e2 = e;
            #endregion

            //Aqui se obtiene la primer columna
            List<int> Cocientes = new List<int>();

            do
            {
                int cociente = phi4 / e;
                int resultado = cociente * e;
                a = phi4 - resultado;
                phi4 = e;
                e = a; //La "a" funciona casi que como un contador
                Cocientes.Add(cociente);
            }
            while (a > 1);

            //Aqui ya se obtiene d
            int[] CocienteArreglo = Cocientes.ToArray();
            do
            {
                for (int i = 0; i < CocienteArreglo.Length; i++) //Realiza el for siempre y cuando el contador no supere el tamaño del arreglo
                {
                    int Producto = CocienteArreglo[i] * d; //Aquí calcula el producto que se usara para la resta
                    int c = phi2 - Producto; //Obtenesmos c la cual pasa a ser la d al final
                    if (c < 0) //Este if sirve para evitar los números negativos
                    {
                        c = phi3 % c; //Se aplica mod de phi siempre
                    }
                    phi2 = d; //Aquí ya va cambiando los valores para seguir con el ciclo
                    d = c; //Cuando el contador supere al arreglo, saldrá del ciclo y se obtendrá la d
                    contador++;
                }
            }
            while (contador < CocienteArreglo.Length);
            string ee = e2.ToString();
            string dd = d.ToString();
            string nn = n.ToString();

            return $"{ee}-{dd}-{nn}";
        }

        public static string CifradoRSA(int Key, int N, string Clave)
        {
            //    BigInteger Potencia = BigInteger.Pow(actual, power);
            //    Mod = (Potencia % N);
            var Public = Key;
            var Private = 0;
            var NN = N;
            var ClaveCifrada = string.Empty;
            foreach (var item in Clave)
            {
                ClaveCifrada += int.Parse(Convert.ToString(BigInteger.ModPow((int)item, Key, NN)));
            }
            return ClaveCifrada;
        }

        public static void DescifradoRSA(int Key, int N, string Nombre)
        {
            var NN = N;
            var Cifrado = new FileStream(PathW, FileMode.Open);
            var Reader = new StreamReader(Cifrado);
            var NuevoNombre = $"{Path.GetDirectoryName(PathW)}\\{Nombre}.txt";
            var Decifrado = new FileStream(NuevoNombre, FileMode.OpenOrCreate);
            var Writer = new BinaryWriter(Decifrado);
            var Line = Reader.ReadLine();
            while (Line != null)
            {
                foreach (var item in Line)
                {
                    Writer.Write((char)int.Parse(Convert.ToString(BigInteger.ModPow((int)item, Key, NN))));
                }
                Line = Reader.ReadLine();
            }
            Decifrado.Close();
            Cifrado.Close();
        }
    }
}
