using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LabReposicion
{
    public class LZWMetodos
    {
        public static void LZWAlgoritmo(string RPath, string WPath, string WPath2)
        {
            Comprimir(RPath, WPath, WPath2);
        }
        public static void LZWAlgoritmo2(string RPath, string WPath)
        {
            descomprimir(RPath, WPath);
        }
        #region Comprimir
        public static void Comprimir(string Rpath, string WPath, string WPath2)
        {
            #region Variables

            int maxDictionaryLenght = 256;
            int byteLenght = 8;

            #endregion

            #region Caracteres

            var diccionario = obtenerDiccionarioCompresion();
            maxDictionaryLenght = diccionario.Count;

            #endregion
            //Analizar texto, creando diccionario y escribiendo en archivo
            #region Algoritmo

            int bufferLength = 1024;

            //Empezar concatenar
            string c = string.Empty;
            List<int> comprimir = new List<int>();

            string bits = "";
            int contador = 0;

            //Buffer para comprimir
            using (var file = new FileStream(Rpath, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(count: bufferLength);

                        foreach (var t in buffer)
                        {
                            string ct = c + ((char)t);
                            if (diccionario.ContainsKey(ct))
                            {
                                c = ct;
                            }
                            else
                            {
                                //sacarlo
                                string ByteString = Convert.ToString(diccionario[c], 2).PadLeft(byteLenght, '0'); // produce cadena "00111111";
                                bits += ByteString;

                                while (bits.Length >= 8) //Escribir bytes en archivo 
                                {
                                    string Byte = bits.Substring(0, 8);
                                    bits = bits.Remove(0, 8);

                                    ByteArrayToFile(WPath, new[] { Convert.ToByte(Convert.ToInt32(Byte, 2)) });
                                }

                                comprimir.Add(diccionario[c]);
                                //Aqui ya lo concatena y lo agrega
                                diccionario.Add(ct, diccionario.Count);
                                c = ((char)t).ToString();
                                //Verificar tamaño de los bits
                                if (diccionario.Count >= maxDictionaryLenght)
                                {
                                    byteLenght++;
                                    maxDictionaryLenght = (int)Math.Pow(2, byteLenght);
                                }
                            }
                        }
                    }

                }

            }
            //Ultima cadena del archivo
            if (!string.IsNullOrEmpty(c))
            {
                string ByteString = Convert.ToString(diccionario[c], 2).PadLeft(byteLenght, '0'); // produce cadena "00111111";
                bits += ByteString;

                comprimir.Add(diccionario[c]);
            }

            if (bits != "") //Bits restantes
            {
                while (bits.Length % 8 != 0)
                {
                    bits += "0";
                }
                while (bits.Length >= 8) //Escribir bytes en archivo 
                {
                    string Byte = bits.Substring(0, 8);
                    bits = bits.Remove(0, 8);

                    ByteArrayToFile(WPath, new[] { Convert.ToByte(Convert.ToInt32(Byte, 2)) });
                }
            }

            #endregion

            #region FileInfo

            FileInfo originalFile = new FileInfo(Rpath);
            FileInfo compressedFile = new FileInfo(WPath);
            MisCompresiones.agregarNuevaCompresion(new MisCompresiones(Path.GetFileName(Rpath), originalFile.Length, compressedFile.Length, WPath2)); //Anadir a mis compresiones

            #endregion
        }
        #endregion
        public static void descomprimir(string RPath, string WPath)
        {


            int bufferLength = 1024;

            int byteLenght = 8;
            int maxDictionaryLenght = 256;

            string bits = "";

            Dictionary<int, char> dictionary = new Dictionary<int, char>();

            using (var file = new FileStream(RPath, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    //Definir diccionario

                    Dictionary<int, string> diccionario = obtenerDiccionarioDescompresion();

                    //Operacion inicial

                    int key = 0;
                    string c = diccionario[(int)reader.ReadByte()];
                    string descomprimir = c;

                    //Buffer para descomprimir
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(count: bufferLength);

                        foreach (var t in buffer)
                        {

                            if (diccionario.Count + 1 >= maxDictionaryLenght)
                            {
                                byteLenght++;
                                maxDictionaryLenght = (int)Math.Pow(2, byteLenght);
                            }

                            string ByteString = Convert.ToString(t, 2).PadLeft(8, '0'); // produce cadena "00111111";
                            bits += ByteString;

                            if (bits.Length >= byteLenght)
                            {
                                key = Convert.ToInt32(bits.Substring(0, byteLenght), 2);
                                bits = bits.Remove(0, byteLenght);

                                string entry = null;
                                if (diccionario.ContainsKey(key))
                                {
                                    entry = diccionario[key];
                                }
                                else if (key == diccionario.Count)
                                {
                                    entry = c + c[0];
                                }

                                descomprimir += entry;

                                //  Agregar nueva frase al diccionario

                                if (entry != null)
                                {
                                    diccionario.Add(diccionario.Count, c + entry[0]);
                                }

                                c = entry;

                            }
                        }

                        Lectura.Escritura(descomprimir, WPath);
                        descomprimir = "";

                    }
                }
            }

        }
        private static Dictionary<int, string> obtenerDiccionarioDescompresion()
        {
            Dictionary<int, string> diccionario = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
            {
                diccionario.Add(i, ((char)i).ToString());
            }

            return diccionario;
        }

        private static Dictionary<string, int> obtenerDiccionarioCompresion()
        {
            Dictionary<string, int> diccionario = new Dictionary<string, int>();
            for (int i = 0; i < 256; i++)
            {
                diccionario.Add(((char)i).ToString(), i);
            }

            return diccionario;
        }

        private static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception caught in process: {0}", ex);
            }
        }

    }
}
