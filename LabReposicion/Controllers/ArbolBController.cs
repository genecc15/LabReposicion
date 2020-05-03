using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LabReposicion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArbolBController : ControllerBase
    {
        public static ArbolB Metodos = new ArbolB();
        public static NodoB Arbol = Singleton.Instance.Raiz;
        [HttpPost]
        [Route("Insertar")]
        public ActionResult<string> Insertar([FromBody] object Bebidanueva)
        {

            var lol = JsonConvert.DeserializeObject<Bebida>(Bebidanueva.ToString());
            lol.Nombre = lol.Nombre.ToLower();
            if (Singleton.Instance.Diccionario.ContainsKey(lol.Nombre))
            {
                return "Dato ya insertado";
            }
            else
            {
                Metodos.Insertar(lol);
                return "Insertado";
                //
            }
        }

        [HttpGet]
        [Route("All")]
        public ActionResult<string> All()
        {
            return JsonConvert.SerializeObject(Singleton.Instance.MostrarArbol.ToArray());
        }

        //Solo un producto
        [HttpGet]
        [Route("Find/{Nombre}")]
        public ActionResult<string> Find(string Nombre)
        {
            var buscar = new Bebida
            {
                Nombre = Nombre
            };
            try
            {
                //buscar en arbol
                return JsonConvert.SerializeObject(Metodos.Buscar(buscar, Singleton.Instance.Raiz));
            }
            catch (Exception)
            {
                Nombre = Nombre.ToLower();
                var json = string.Empty;
                if (Singleton.Instance.Diccionario.ContainsKey(Nombre))
                {
                    json = JsonConvert.SerializeObject(Singleton.Instance.Diccionario[Nombre]);
                }
                else
                {
                    json = "El Elemento No existe";

                }
                return json;
            }
        }
    }
}