using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LabReposicion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Variables
    {
        public string Cidrado { get; set; }
        public string Clave { get; set; }
        public string Path { get; set; }
        public string NuevoNombre { get; set; }
        public int p { get; set; }
        public int q { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string N { get; set; }
    }
    public class RSAController : ControllerBase
    {
        private const int bufferLength = 1024;
        public static string CurrentFile = "";


        [HttpPost("CifrarCesar")]

        public void CifrarArchivo([FromBody] object obj)
        {
            var variables = JsonConvert.DeserializeObject<Variables>(obj.ToString());
            if(variables.PublicKey==null)
            {
                var Clave = RSA.DescifradoRSA(int.Parse(variables.PrivateKey), int.Parse(variables.N), variables.Clave);
                Cifrado(variables.Path, variables.NuevoNombre, Clave);
            }
        }

        [HttpPost("DescifrarCesar")]
        public void DescifrarArchivo([FromBody]object obj)
        {
            var variables = JsonConvert.DeserializeObject<Variables>(obj.ToString());
            if(variables.PublicKey==null)
            {
                var Clave = RSA.DescifradoRSA(int.Parse(variables.PrivateKey), int.Parse(variables.N), variables.Clave);
                Descifrar(variables.Path, variables.NuevoNombre, Clave);

            }
            else
            {
                var Clave = RSA.DescifradoRSA(int.Parse(variables.PublicKey), int.Parse(variables.N), variables.Clave);
                Descifrar(variables.Path, variables.NuevoNombre, Clave); 
            }
        }
        #region Cifrar

        public static void Cifrado(string Rpath, string Nombre, string llave)
        {
            #region Alfabeto Mayusculas
            List<char> AlfabetoM = new List<char>();
            AlfabetoM.Add('A');
            AlfabetoM.Add('B');
            AlfabetoM.Add('C');
            AlfabetoM.Add('D');
            AlfabetoM.Add('E');
            AlfabetoM.Add('F');
            AlfabetoM.Add('G');
            AlfabetoM.Add('H');
            AlfabetoM.Add('I');
            AlfabetoM.Add('J');
            AlfabetoM.Add('K');
            AlfabetoM.Add('L');
            AlfabetoM.Add('M');
            AlfabetoM.Add('N');
            AlfabetoM.Add('O');
            AlfabetoM.Add('P');
            AlfabetoM.Add('Q');
            AlfabetoM.Add('R');
            AlfabetoM.Add('S');
            AlfabetoM.Add('T');
            AlfabetoM.Add('U');
            AlfabetoM.Add('V');
            AlfabetoM.Add('W');
            AlfabetoM.Add('X');
            AlfabetoM.Add('Y');
            AlfabetoM.Add('Z');
            AlfabetoM.Add(' ');
            #endregion
            #region Alfabeto Minusculas
            List<char> AlfabetoP = new List<char>();
            AlfabetoP.Add('a');
            AlfabetoP.Add('b');
            AlfabetoP.Add('c');
            AlfabetoP.Add('d');
            AlfabetoP.Add('e');
            AlfabetoP.Add('f');
            AlfabetoP.Add('g');
            AlfabetoP.Add('h');
            AlfabetoP.Add('i');
            AlfabetoP.Add('j');
            AlfabetoP.Add('k');
            AlfabetoP.Add('l');
            AlfabetoP.Add('m');
            AlfabetoP.Add('n');
            AlfabetoP.Add('o');
            AlfabetoP.Add('p');
            AlfabetoP.Add('q');
            AlfabetoP.Add('r');
            AlfabetoP.Add('s');
            AlfabetoP.Add('t');
            AlfabetoP.Add('u');
            AlfabetoP.Add('v');
            AlfabetoP.Add('w');
            AlfabetoP.Add('x');
            AlfabetoP.Add('y');
            AlfabetoP.Add('z');
            AlfabetoP.Add(' ');
            #endregion

            string data = System.IO.File.ReadAllText(Rpath, Encoding.Default);
            #region Listas
            string clave = llave.ToUpper();
            string claveM = llave.ToLower();
            List<char> ListaFinal = new List<char>(); //Lista que sera el abecedario modificado
            List<char> ListaFinal2 = new List<char>();
            List<char> ListaClave = clave.ToList();
            List<char> ListaClave2 = claveM.ToList();
            #endregion
            #region Diccionario Mayusculas
            List<char> Diferentes = AlfabetoM.Except(ListaClave).ToList();
            List<char> Repetidos = (ListaClave.AsQueryable().Intersect(AlfabetoM)).ToList(); //Expresion Lamba para encontrar repetidos
            ListaFinal = Repetidos.Union(Diferentes).ToList();
            var DiccionarioM = AlfabetoM.Zip(ListaFinal, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v); //Combinar listas y volverlas diccionario
            #endregion

            #region Diccionario Minusculas
            List<char> DiferentesP = AlfabetoP.Except(ListaClave2).ToList();
            List<char> RepetidosP = (ListaClave2.AsQueryable().Intersect(AlfabetoP)).ToList(); //Expresion Lamba para encontrar repetidos
            ListaFinal2 = RepetidosP.Union(DiferentesP).ToList();
            var DiccionarioP = AlfabetoP.Zip(ListaFinal2, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v); //Combinar listas y volverlas diccionario
            #endregion
            var OP = new FileStream(Rpath, FileMode.Open);
            var reader2 = new StreamReader(OP);
            var NuevoNombre = $"{Path.GetDirectoryName(OP.Name)}\\{Nombre}.txt";
            var creation = new FileStream(NuevoNombre, FileMode.OpenOrCreate);
            var Write = new BinaryWriter(creation);

            using (var file = new FileStream(Rpath, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    //Buffer para cifrar
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(count: bufferLength);

                        List<char> Cifrado = new List<char>();
                        List<byte> CifradoFinal = new List<byte>();

                        foreach (var item in buffer)
                        {
                            Cifrado.Add((char)item);
                        }
                        foreach (var item in Cifrado)
                        {
                            if (DiccionarioM.ContainsKey(item))
                            {
                                CifradoFinal.Add((byte)DiccionarioM[item]);
                            }
                            else if (DiccionarioP.ContainsKey(item))
                            {
                                CifradoFinal.Add((byte)DiccionarioP[item]);
                            }
                            else
                            {
                                CifradoFinal.Add((byte)item);
                            }
                        }
                        foreach(var item in CifradoFinal)
                        {
                            Write.Write(item);
                        }

                        OP.Close();
                        creation.Close();
                    }
                }
            }

        }
        #endregion

        #region Descifrar
        public static void Descifrar(string Rpath, string Nombre, string llave)
        {

            #region Alfabeto Mayusculas
            List<char> AlfabetoM = new List<char>();
            AlfabetoM.Add('A');
            AlfabetoM.Add('B');
            AlfabetoM.Add('C');
            AlfabetoM.Add('D');
            AlfabetoM.Add('E');
            AlfabetoM.Add('F');
            AlfabetoM.Add('G');
            AlfabetoM.Add('H');
            AlfabetoM.Add('I');
            AlfabetoM.Add('J');
            AlfabetoM.Add('K');
            AlfabetoM.Add('L');
            AlfabetoM.Add('M');
            AlfabetoM.Add('N');
            AlfabetoM.Add('O');
            AlfabetoM.Add('P');
            AlfabetoM.Add('Q');
            AlfabetoM.Add('R');
            AlfabetoM.Add('S');
            AlfabetoM.Add('T');
            AlfabetoM.Add('U');
            AlfabetoM.Add('V');
            AlfabetoM.Add('W');
            AlfabetoM.Add('X');
            AlfabetoM.Add('Y');
            AlfabetoM.Add('Z');
            AlfabetoM.Add(' ');
            #endregion
            #region Alfabeto Minusculas
            List<char> AlfabetoP = new List<char>();
            AlfabetoP.Add('a');
            AlfabetoP.Add('b');
            AlfabetoP.Add('c');
            AlfabetoP.Add('d');
            AlfabetoP.Add('e');
            AlfabetoP.Add('f');
            AlfabetoP.Add('g');
            AlfabetoP.Add('h');
            AlfabetoP.Add('i');
            AlfabetoP.Add('j');
            AlfabetoP.Add('k');
            AlfabetoP.Add('l');
            AlfabetoP.Add('m');
            AlfabetoP.Add('n');
            AlfabetoP.Add('o');
            AlfabetoP.Add('p');
            AlfabetoP.Add('q');
            AlfabetoP.Add('r');
            AlfabetoP.Add('s');
            AlfabetoP.Add('t');
            AlfabetoP.Add('u');
            AlfabetoP.Add('v');
            AlfabetoP.Add('w');
            AlfabetoP.Add('x');
            AlfabetoP.Add('y');
            AlfabetoP.Add('z');
            AlfabetoP.Add(' ');
            #endregion

            string data = System.IO.File.ReadAllText(Rpath, Encoding.Default);
            #region Listas
            string clave = llave.ToUpper();
            string claveM = llave.ToLower();
            List<char> ListaFinal = new List<char>(); //Lista que sera el abecedario modificado
            List<char> ListaFinal2 = new List<char>();
            List<char> ListaClave = clave.ToList();
            List<char> ListaClave2 = claveM.ToList();
            #endregion
            #region Diccionario Mayusculas
            List<char> Diferentes = AlfabetoM.Except(ListaClave).ToList();
            List<char> Repetidos = (ListaClave.AsQueryable().Intersect(AlfabetoM)).ToList(); //Expresion Lamba para encontrar repetidos
            ListaFinal = Repetidos.Union(Diferentes).ToList();
            var DiccionarioM = ListaFinal.Zip(AlfabetoM, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v); //Combinar listas y volverlas diccionario
            #endregion
            #region Diccionario Minusculas
            List<char> DiferentesP = AlfabetoP.Except(ListaClave2).ToList();
            List<char> RepetidosP = (ListaClave2.AsQueryable().Intersect(AlfabetoP)).ToList(); //Expresion Lamba para encontrar repetidos
            ListaFinal2 = RepetidosP.Union(DiferentesP).ToList();
            var DiccionarioP = ListaFinal2.Zip(AlfabetoP, (k, v) => new { k, v }).ToDictionary(x => x.k, x => x.v); //Combinar listas y volverlas diccionario
            #endregion
            var Cifrado = new FileStream(Rpath, FileMode.Open);
            var NuevoNombre = $"{Path.GetDirectoryName(Cifrado.Name)}\\{Nombre}Descomprimido.txt";
            var creation = new FileStream(NuevoNombre, FileMode.OpenOrCreate);
            var write = new BinaryWriter(creation);
            using (var file = new FileStream(Rpath, FileMode.Open))
            {
                using (var reader = new BinaryReader(file))
                {
                    //Buffer para descifrar
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var buffer = reader.ReadBytes(count: bufferLength);
                        List<byte> DescifradoFinal = new List<byte>();

                        foreach (var item in buffer)
                        {
                            if (DiccionarioM.ContainsKey((char)item))
                            {
                                DescifradoFinal.Add((byte)DiccionarioM[(char)item]);
                            }
                            else if (DiccionarioP.ContainsKey((char)item))
                            {
                                DescifradoFinal.Add((byte)DiccionarioP[(char)item]);
                            }
                            else
                            {
                                DescifradoFinal.Add(item);
                            }
                        }
                        foreach (var item in DescifradoFinal)
                        {
                            write.Write(item);
                        }
                    }
                }
            }
        }
#endregion
    }
}