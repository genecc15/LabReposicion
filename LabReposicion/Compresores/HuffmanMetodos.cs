using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text;

namespace LabReposicion
{
    public class HuffmanMetodos
    {
        #region Estructuras
        private static decimal TotalData;
        private const int BufferLenght = 1024;
        private static char Separator = new char();
        private static Nodo Root { get; set; }
        private static Dictionary<byte, int> FrequencyTable { get; set; }
        private static Dictionary<byte, string> CharacterTable { get; set; }
        private static Dictionary<string, byte> CharacterTable2 { get; set; }
        private static Dictionary<byte, int> FrequencyTable2 { get; set; }

        public static int DataSize { get; set; }
        public static decimal DataLenght;
        #endregion

        #region Comprimir
        public static void Comprimir(string RPath, string WPath, string originalName, string WPath2)
        {
            FrequencyTable = new Dictionary<byte, int>();
            CharacterTable = new Dictionary<byte, string>();
            HuffmanTree(RPath);
            GetPrefixCodes();
            ValueFrequencyWrite(WPath, originalName);
            Recorre(RPath, WPath);
            #region FileInfo

            FileInfo originalFile = new FileInfo(RPath);
            FileInfo compressedFile = new FileInfo(WPath);
            MisCompresiones.agregarNuevaCompresion(new MisCompresiones(Path.GetFileName(RPath), originalFile.Length, compressedFile.Length, WPath2)); //Anadir a mis compresiones

            #endregion
        }
        private static void HuffmanTree(string path)
        {
            using (var File = new FileStream(path, FileMode.Open))
            {
                var Buffer = new byte[BufferLenght];
                using var Reader = new BinaryReader(File);
                TotalData = Reader.BaseStream.Length;

                while (Reader.BaseStream.Position != Reader.BaseStream.Length)
                {
                    Buffer = Reader.ReadBytes(BufferLenght);

                    foreach (var item in Buffer)
                    {
                        if (FrequencyTable.Keys.Contains(item)) FrequencyTable[(item)]++;
                        else FrequencyTable.Add(item, 1);
                    }
                }
            }

            List<Nodo> FrequencyList = new List<Nodo>();

            foreach (KeyValuePair<byte, int> Nodes in FrequencyTable)
            {
                FrequencyList.Add(new Nodo(Nodes.Key, Convert.ToDecimal(Nodes.Value) / TotalData));
            }

            while (FrequencyList.Count > 1)
            {
                if (FrequencyList.Count == 1) break;
                else
                {
                    FrequencyList = FrequencyList.OrderBy(x => x.Probability).ToList();
                    Nodo Union = LinkNodes(FrequencyList[1], FrequencyList[0]);
                    FrequencyList.RemoveRange(0, 2);
                    FrequencyList.Add(Union);
                }
            }

            Root = FrequencyList[0];
        }

        private static Nodo LinkNodes(Nodo Mayor, Nodo Menor)
        {
            var Parent = new Nodo(Mayor.Probability + Menor.Probability);
            Parent.LeftNode = Mayor;
            Parent.RightNode = Menor;
            return Parent;
        }

        private static void Recorre(string ReadingPath, string WritingPath)
        {
            string Recorrido = "";
            using var writer = new FileStream(WritingPath, FileMode.Append);
            using var File = new FileStream(ReadingPath, FileMode.Open);
            var buffer = new byte[BufferLenght];
            var Bytes = new List<byte>();
            using var reader = new BinaryReader(File);

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                buffer = reader.ReadBytes(BufferLenght);

                foreach (var item in buffer)
                {
                    Recorrido += CharacterTable[item];

                    if (Recorrido.Length >= 8)
                    {
                        while (Recorrido.Length > 8)
                        {
                            Bytes.Add(Convert.ToByte(Recorrido.Substring(0, 8), 2));
                            Recorrido = Recorrido.Remove(0, 8);
                        }
                    }
                }

                writer.Write(Bytes.ToArray(), 0, Bytes.ToArray().Length);
                Bytes.Clear();
            }

            for (int i = Recorrido.Length; i < 8; i++)
            {
                Recorrido += "0";
            }

            Bytes.Add(Convert.ToByte(Recorrido, 2));
            writer.Write(Bytes.ToArray(), 0, Bytes.ToArray().Length);
        }

        private static void ValueFrequencyWrite(string path, string originalName)
        {
            var Writing = new byte[BufferLenght];
            Separator = '|';

            if (CharacterTable.Keys.Contains(Convert.ToByte('|')))
            {
                Separator = 'ÿ';
                if (CharacterTable.Keys.Contains(Convert.ToByte('ÿ'))) Separator = 'ß';
            }

            using var originalNameWriter = new StreamWriter(path);
            originalNameWriter.WriteLine(originalName);
            originalNameWriter.Close();
            using var file = new FileStream(path, FileMode.OpenOrCreate);
            using var Writer = new BinaryWriter(file);
            Writer.Seek(0, SeekOrigin.End);
            Writing = Encoding.UTF8.GetBytes(TotalData.ToString().ToArray());
            Writer.Write(Writing);
            Writer.Write(Convert.ToByte(Separator));

            foreach (KeyValuePair<byte, int> Values in FrequencyTable)
            {
                Writer.Write(Values.Key);
                Writing = Encoding.UTF8.GetBytes(Values.Value.ToString().ToArray());
                Writer.Write(Writing);
                Writer.Write(Convert.ToByte(Separator));
            }

            Writer.Write(Convert.ToByte(Separator));
        }

        private static void PrefixCodes(Nodo Node, string Route)
        {
            if (Node.IsLeaf()) CharacterTable.Add(Node.Fact, Route);
            else
            {
                if (Node.LeftNode != null) PrefixCodes(Node.LeftNode, Route + "0");
                if (Node.RightNode != null) PrefixCodes(Node.RightNode, Route + "1");
            }
        }

        private static void GetPrefixCodes()
        {
            if (Root.IsLeaf()) CharacterTable.Add(Root.Fact, "1");
            else PrefixCodes(Root, "");
        }
        #endregion

        #region Descomprimir
        public static void Descomprimir(string RPath, string WPath)
        {
            FrequencyTable2 = new Dictionary<byte, int>();
            CharacterTable2 = new Dictionary<string, byte>();
            HuffmanTree2(RPath);
            GetPrefixCodes2();
            Desc(RPath, WPath);
        }

        static void HuffmanTree2(string path)
        {
            using var nameJumper = new StreamReader(path);
            var position = nameJumper.ReadLine().Length;
            nameJumper.Close();

            using (var File = new FileStream(path, FileMode.Open))
            {
                File.Position = position + 1;
                int separator1 = 0;
                var buffer = new byte[BufferLenght];
                string Data_Lenght1 = "";
                string frequency = "";
                string Datamount = "";
                int final = 0;
                byte bit = new byte();
                using (var reader = new BinaryReader(File))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(BufferLenght);
                        foreach (var item in buffer)
                        {

                            if (separator1 == 0)
                            {
                                if (Convert.ToChar(item) == '|' || Convert.ToChar(item) == 'ÿ' || Convert.ToChar(item) == 'ß')
                                {
                                    separator1 = 1;
                                    if (Convert.ToChar(item) == '|') Separator = '|';
                                    else if (Convert.ToChar(item) == 'ÿ') Separator = 'ÿ';
                                    else Separator = 'ß';
                                }
                                else Data_Lenght1 += Convert.ToChar(item).ToString();
                            }
                            else if (separator1 == 2) break;
                            else
                            {
                                if (final == 1 && Convert.ToChar(item) == Separator)
                                {
                                    final = 2;
                                    separator1 = 2;
                                }
                                else final = 0;

                                if (Datamount == "") { Datamount = Convert.ToChar(item).ToString(); bit = item; }
                                else if (Convert.ToChar(item) == Separator && final == 0)
                                {
                                    FrequencyTable2.Add(bit, Convert.ToInt32(frequency));
                                    Datamount = "";
                                    frequency = "";
                                    final = 1;
                                }
                                else frequency += Convert.ToChar(item).ToString();
                            }

                        }
                    }
                }

                DataLenght = Convert.ToDecimal(Data_Lenght1);
            }

            List<Nodo> FrequencyList = new List<Nodo>();

            foreach (KeyValuePair<byte, int> Nodes in FrequencyTable2)
            {
                FrequencyList.Add(new Nodo(Nodes.Key, Convert.ToDecimal(Nodes.Value) / DataLenght));
            }

            FrequencyList = FrequencyList.OrderBy(x => x.Probability).ToList();

            while (FrequencyList.Count > 1)
            {
                FrequencyList = FrequencyList.OrderBy(x => x.Probability).ToList();
                Nodo Link = LinkNodes(FrequencyList[1], FrequencyList[0]);
                FrequencyList.RemoveRange(0, 2);
                FrequencyList.Add(Link);
            }

            Root = FrequencyList[0];
        }

        private static void PrefixCodes2(Nodo Node, string desc)
        {
            if (Node.IsLeaf()) { CharacterTable2.Add(desc, Node.Fact); return; }
            else
            {
                if (Node.LeftNode != null) PrefixCodes2(Node.LeftNode, desc + "0");
                if (Node.RightNode != null) PrefixCodes2(Node.RightNode, desc + "1");
            }
        }

        public static Nodo LinkNodes2(Nodo Mayor, Nodo Menor)
        {
            Nodo Parent = new Nodo(Mayor.Probability + Menor.Probability);
            Parent.LeftNode = Mayor;
            Parent.RightNode = Menor;
            return Parent;
        }

        private static void GetPrefixCodes2()
        {
            if (Root.IsLeaf()) CharacterTable2.Add("1", Root.Fact);
            else PrefixCodes2(Root, "");
        }

        private static void Desc(string ReadingPath, string WritingPath)
        {
            int written = 0;
            int i = 0;
            string validation = "";
            int start = 0;
            string desc = "";
            List<byte> bites = new List<byte>();
            using var file = new FileStream(WritingPath, FileMode.OpenOrCreate);
            using var writer = new BinaryWriter(file);
            using var nameJumper = new StreamReader(ReadingPath);
            var position = nameJumper.ReadLine().Length;
            nameJumper.Close();
            using var File = new FileStream(ReadingPath, FileMode.Open);
            File.Position = position + 1;
            var buffer = new byte[BufferLenght];
            using var reader = new BinaryReader(File);

            while (reader.BaseStream.Position != reader.BaseStream.Length && written < DataLenght)
            {
                buffer = reader.ReadBytes(BufferLenght);

                foreach (var item in buffer)
                {
                    written++;
                    if (start == 0 && Convert.ToChar(item) == Separator) start = 1;
                    else if (start == 1 && Convert.ToChar(item) == Separator) start = 2;
                    else if (start == 2)
                    {
                        var bits = Convert.ToString(item, 2);
                        var full = bits.PadLeft(8, '0');
                        desc += full;
                        var comparation = desc.ToCharArray();
                        i = 0;

                        while (i < desc.Length)
                        {
                            validation += comparation[i];
                            i++;

                            if (CharacterTable2.Keys.Contains(validation))
                            {
                                i = 0;
                                bites.Add(CharacterTable2[validation]);
                                desc = desc.Remove(0, validation.Length);
                                comparation = desc.ToCharArray();
                                validation = "";
                            }
                        }

                        validation = "";
                    }
                    else start = 0;
                }

                writer.Write(bites.ToArray());
                bites.Clear();
            }
        }
        #endregion
    }
}
