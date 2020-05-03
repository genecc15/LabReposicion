using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LabReposicion.Compressor;

namespace LabReposicion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HuffmanController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        private readonly HuffmanMetodos HuffCompress = new HuffmanMetodos();
        readonly HuffmanMetodos HuffDesc = new HuffmanMetodos();

        public HuffmanController(IWebHostEnvironment env)
        {
            _environment = env;
        }

        public class FileUploadAPI
        {
          public IFormFile Files { get; set; }
        }

        [Route("/Compressions/Huffman")]
        [HttpGet]
        public async Task<IActionResult> DownloadCompressions()
        {
            var memory = new MemoryStream();

            using (var stream = new FileStream(_environment.WebRootPath + "\\UploadHuffman\\" + "Compresiones.txt", FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }

            memory.Position = 0;
            return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, "Compresiones.txt");

        }
        [Route("/Compress/{id}/Huffman")]
        [HttpPost]
        public async Task<IActionResult> UploadFileText([FromForm] FileUploadAPI objFile, string id)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadHuffman\\")) Directory.CreateDirectory(_environment.WebRootPath + "\\UploadHuffman\\");
                    using var _fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadHuffman\\" + objFile.Files.FileName);
                    objFile.Files.CopyTo(_fileStream);
                    _fileStream.Flush();
                    _fileStream.Close();

                    HuffmanCompress(objFile, id);
                    var memory = new MemoryStream();

                    using (var stream = new FileStream(_environment.WebRootPath + "\\UploadHuffman\\" + id + ".huff", FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, id + ".huff");
                }

                return StatusCode(404, "Archivo Vacio");

            }
            catch
            {
                return StatusCode(404, "Error");
            }
        }

        public void HuffmanCompress(FileUploadAPI objFile, string id)
        {
            string[] FileName1 = objFile.Files.FileName.Split(".");
            HuffmanMetodos.Comprimir(_environment.WebRootPath + "\\UploadHuffman\\" + objFile.Files.FileName, _environment.WebRootPath + "\\UploadHuffman\\" + id + ".huff", FileName1[0], _environment.WebRootPath + "\\UploadHuffman\\" + "Compresiones.txt");
          
        }

        [Route("/Decompress/Huffman")]
        [HttpPost]
        public async Task<IActionResult> UploadFileHuff([FromForm] FileUploadAPI objFile)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadHuffman\\")) Directory.CreateDirectory(_environment.WebRootPath + "\\UploadHuffman\\");
                    using var _fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadHuffman\\" + objFile.Files.FileName);
                    objFile.Files.CopyTo(_fileStream);
                    _fileStream.Flush();
                    _fileStream.Close();
                    HuffmanDescompress(objFile);

                    var memory = new MemoryStream();

                    using (var stream = new FileStream(_environment.WebRootPath + "\\UploadHuffman\\" + objFile.Files.FileName + ".txt", FileMode.Open))
                    {
                        await stream.CopyToAsync(memory);
                    }

                    memory.Position = 0;
                    return File(memory, System.Net.Mime.MediaTypeNames.Application.Octet, objFile.Files.FileName + ".txt");
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

        public void HuffmanDescompress(FileUploadAPI objFile)
        {
            string[] FileName1 = objFile.Files.FileName.Split(".");
            HuffmanMetodos.Descomprimir(_environment.WebRootPath + "\\UploadHuffman\\" + objFile.Files.FileName, _environment.WebRootPath + "\\UploadHuffman\\" + FileName1[0] + ".txt");

        }

    }
}