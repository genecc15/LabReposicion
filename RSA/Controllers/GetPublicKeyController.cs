using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace RSA.Controllers
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
    public class GetPublicKeyController : ControllerBase
    {
        static string PublicKey;
        static string PrivateKey;
        static string N;
        static bool Llave = false;
        [HttpPost("GetPublicKey")]
        public string Upload([FromBody] object obj)
        {
            var Variables = JsonConvert.DeserializeObject<Variables>(obj.ToString());
            var Keys = RSA.GenerarLlaves(Variables.p, Variables.q).Split('-');
            PublicKey = Keys[0];
            PrivateKey = Keys[1];
            N = Keys[2];

            Llave = true;

            return $"Llave Publica: {Keys[0]}, N: {Keys[2]}"; 
        }

        [HttpPost("Cifrar")]
        public async Task<IActionResult> CifradoPublicKey([FromBody] object obj)
        {
            if(Llave)
            {
                var Variables = JsonConvert.DeserializeObject<Variables>(obj.ToString());
                Variables.Clave = RSA.CifradoRSA(int.Parse(PublicKey), int.Parse(N), Variables.Clave);
                Variables.N = N;
                Variables.PublicKey = null;
                Variables.PrivateKey = PrivateKey;
                var Json = JsonConvert.SerializeObject(Variables);
                var client = new HttpClient();
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                var respose = await client.PostAsync("https://localhost:44313/api/RSA/CifrarCesar", content);

                return StatusCode(202, "Cifrado Correctamente");
            }
            else
            {
                return StatusCode(404, "Genere llaves");
            }
        }

        public async Task<IActionResult> DecifrarPublicKey([FromBody]object obj)
        {
           if(Llave)
            {
                var Variables = JsonConvert.DeserializeObject<Variables>(obj.ToString());
                Variables.Clave = RSA.CifradoRSA(int.Parse(PublicKey), int.Parse(N), Variables.Clave);
                Variables.N = N;
                Variables.PublicKey = null;
                Variables.PrivateKey = PrivateKey;
                var Json = JsonConvert.SerializeObject(Variables);
                var client = new HttpClient();
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                var respose = await client.PostAsync("https://localhost:44313/api/RSA/DescifrarCesar", content);
                return StatusCode(202, "Descifrado Correctamente");
            }
            else
            {
                return StatusCode(404, "Genere llaves");
            }

        }
    }
}