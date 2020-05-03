using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace LabReposicion
{
    public class Lectura
    {
        private const int bufferLength = 1024;

        #region Default

        public void Leer(int lenght, string path)
        {
            List<char> bufferList = new List<char>();

            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(bufferLength);

                        foreach (var item in buffer)
                        {
                            bufferList.Add((char)item);
                        }
                    }
                }
            }
        }

        public static void Escritura(string text, string path)
        {
            var buffer = new byte[text.Length];

            using (var file = new FileStream(path, FileMode.Append))
            {
                using (var writer = new BinaryWriter(file))
                {
                    for (int i = 0; i < buffer.Length; i++)
                    {
                        buffer[i] = Convert.ToByte(text[i]);
                    }

                    writer.Write(buffer);
                }
            }
        }

        public static void InsertData(string path, int position, byte[] data)
        {
            using (Stream stream = File.Open(path, FileMode.Open))
            {
                stream.Position = position;
                stream.Write(data, 0, data.Length);
            }
        }
        #endregion
        #region LZW

        public static Dictionary<char, int> obtenerDiccionarioLZW(string path)
        {
            var dictionary = new Dictionary<char, int>();
            int hashKey = 1;

            using (var file = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(bufferLength);

                        foreach (var item in buffer)
                        {
                            if (!dictionary.ContainsKey((char)item))
                            {
                                dictionary.Add((char)item, hashKey);
                                hashKey++;
                            }
                        }
                    }
                }
            }

            return dictionary;
        }

        #endregion
    }
}
