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
        private static decimal TotalData;
        private const int BufferLenght = 1024;
        private static char Separator = new char();
        private static Nodo Root { get; set; }
        private static Dictionary<byte, int> FrequencyTable { get; set; }
        private static Dictionary<byte, string> CharacterTable { get; set; }

        public void Comprimir(string ReadingPath, string WritingPath, string originalName)
        {
            FrequencyTable = new Dictionary<byte, int>();
            CharacterTable = new Dictionary<byte, string>();
            HuffmanTree(ReadingPath);
            GetPrefixCodes();
            ValueFrequencyWrite(WritingPath, originalName);
            Recorre(ReadingPath, WritingPath);
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
    }
}
