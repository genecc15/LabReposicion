using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSA
{
    public class NumerosPrimos
    {
        public static bool esNumeroPrimo(int n)
        {
            if (n > 1)
            {
                for (int i = 2; i < n; i++)
                {
                    if (n % i == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public static int obtenerNumeroE(int N, int Phi)
        {
            List<int> factoresN = obtenerCoprimos(N);
            List<int> factoresPhi = obtenerCoprimos(Phi);

            List<int> coprimos = new List<int>();

            foreach (var item in factoresPhi)
            {
                if (factoresN.Contains(item))
                {
                    coprimos.Add(item);
                }
            }

            Random rnd = new Random();
            double range = coprimos.Count / 2;
            int e = coprimos[rnd.Next(0, Convert.ToInt16(Math.Truncate(range)))];

            return e;
        }

        private static List<int> obtenerCoprimos(int n)
        {
            //Lista de numeros
            List<int> numeros = new List<int>();

            for (int i = 2; i < n; i++)
            {
                numeros.Add(i);
            }

            //Factorizar
            List<int> factores = new List<int>();

            if (n % 2 == 0) //si es un numero par
            {
                factores.Add(2);
            }

            for (int i = 3; i < n / 2; i += 2)
            {
                if (n % i == 0)
                {
                    factores.Add(i);
                }
            }

            //Eliminar Multiplos

            foreach (var item in factores)
            {
                eliminarMultiplos(item, ref numeros);
            }

            return numeros;
        }

        private static void eliminarMultiplos(int n, ref List<int> numeros)
        {
            List<int> remove = new List<int>();

            foreach (var item in numeros)
            {
                if (item % n == 0)
                {
                    remove.Add(item);
                }
            }

            numeros = numeros.Except(remove).ToList();
        }
    }
}
