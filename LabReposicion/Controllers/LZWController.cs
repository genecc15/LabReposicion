using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;


namespace LabReposicion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LZWController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        private readonly LZWMetodos LZWCompresion = new LZWMetodos();
        private readonly LZWMetodos LZWDesc = new LZWMetodos();

        public LZWController(IWebHostEnvironment env)
        {
            _environment = env;
        }
        public class FileUploadAPI
        {
            public IFormFile Files { get; set; }
        }

        [Route("/Compressions/LZW")]
        [HttpGet]
        public async Task<IActionResult> DownloadCompressions()
        {
            var memory = new MemoryStream();

            using (var stream = new FileStream(_environment.WebRootPath + "\\UploadLZW\\" + "Compresiones.txt", FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, "Compresiones.txt");
        }

        [Route("/Compress/{id}/LZW")]
        [HttpPost]
        public async Task<IActionResult> UploadFileText([FromForm] FileUploadAPI objFile, string id)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadLZW\\")) Directory.CreateDirectory(_environment.WebRootPath + "\\UploadLZW\\");
                    using var _fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName);
                    objFile.Files.CopyTo(_fileStream);
                    _fileStream.Flush();
                    _fileStream.Close();

                    LZWCompress(objFile, id);
                    var memory = new MemoryStream();

                    using (var stream = new FileStream(_environment.WebRootPath + "\\UploadLZW\\" + id + ".lzw", FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, id + ".lzw");
                }
                else return StatusCode(404, "Vacio");

            }
            catch
            {
                return StatusCode(404, "Error");
            }
        }
        public void LZWCompress(FileUploadAPI objFile, string id)
        {
            string[] FileName1 = objFile.Files.FileName.Split(".");
            LZWMetodos.LZWAlgoritmo(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName, _environment.WebRootPath + "\\UploadLZW\\" + id + ".lzw", _environment.WebRootPath + "\\UploadLZW\\" + "Compresiones.txt");
        }
        [Route("/Decompress/LZW")]
        [HttpPost]
        public async Task<IActionResult> UploadFileLZW([FromForm] FileUploadAPI objFile)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadLZW\\")) Directory.CreateDirectory(_environment.WebRootPath + "\\UploadLZW\\");
                    using var _fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName);
                    objFile.Files.CopyTo(_fileStream);
                    _fileStream.Flush();
                    _fileStream.Close();
                    LZWDecompress(objFile);
                    var memory = new MemoryStream();

                    using (var stream = new FileStream(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName + ".txt", FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, objFile.Files.FileName + objFile.Files.FileName + ".txt");


                }
                else
                {
                    return StatusCode(404, "Archivo Vacio");
                }
            }
            catch
            {
                return StatusCode(404, "Error");
            }
        }

        public void LZWDecompress(FileUploadAPI LZWFile)
        {

            string[] FileName1 = LZWFile.Files.FileName.Split(".");
            LZWMetodos.LZWAlgoritmo2(_environment.WebRootPath + "\\UploadLZW\\" + LZWFile.Files.FileName, _environment.WebRootPath + "\\UploadLZW\\" + FileName1[0] + ".txt");

        }
    }
}