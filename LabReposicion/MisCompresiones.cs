using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LabReposicion
{
    public class MisCompresiones
    {
        public string nombreOriginal { get; set; }
        public string RutaO { get; set; }
        public double razonDeCompresion { get; set; }
        public double factorDeCompresion { get; set; }
        public double porcentajeDeCompresion { get; set; }

        public MisCompresiones() { }
        public MisCompresiones(string nombre, double razon, double factor, double porcentaje, string Ruta)
        {
            nombreOriginal = nombre;
            razonDeCompresion = razon;
            factorDeCompresion = factor;
            porcentajeDeCompresion = porcentaje;
            RutaO = Ruta;
        }
        public MisCompresiones(string nombre, long pesoOriginal, long pesoComprimido, string Path)
        {
            nombreOriginal = nombre;
            razonDeCompresion = calcularRazon(pesoOriginal, pesoComprimido);
            factorDeCompresion = calcularFactor(pesoOriginal, pesoComprimido);
            porcentajeDeCompresion = calcularPorcentaje(pesoOriginal, pesoComprimido);
            RutaO = Path;
        }

        //Calcular valores solicitados...
        private double calcularRazon(long pesoOriginal, long pesoComprimido)
        {
            return Math.Round(Convert.ToDouble(pesoComprimido) / Convert.ToDouble(pesoOriginal), 2);
        }

        private double calcularFactor(long pesoOriginal, long pesoComprimido)
        {
            return Math.Round(Convert.ToDouble(pesoOriginal) / Convert.ToDouble(pesoComprimido), 2);
        }

        private double calcularPorcentaje(long pesoOriginal, long pesoComprimido)
        {
            return Math.Round(100 - ((Convert.ToDouble(pesoComprimido) / Convert.ToDouble(pesoOriginal)) * 100), 2);
        }

        public static void agregarNuevaCompresion(MisCompresiones nuevo)
        {


            string path = nuevo.RutaO;
            using (StreamWriter sw = File.AppendText(path))
            {
                string text =
                    $"Nombre Original: {nuevo.nombreOriginal}, Razon de Compresion: {nuevo.razonDeCompresion}, Factor de Compresion: {nuevo.factorDeCompresion}, Porcentaje de Compresion: {nuevo.porcentajeDeCompresion}";
                sw.WriteLine(text);
            }

        }

    }
}
