using System.Net;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Business;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PersonApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class FilesController : ControllerBase
    {
        private readonly IFileBusiness _fileBusiness;

        public FilesController(IFileBusiness fileBusiness)
        {
            _fileBusiness = fileBusiness;
        }

        [HttpGet]
        [SwaggerResponse(200, Type = typeof(byte []))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        public IActionResult GetPDFFile()
        {
            byte[] buffer = _fileBusiness.GetPDFFile();

            if (buffer != null)
            {
                HttpContext.Response.ContentType = "application/pdf";
                HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                HttpContext.Response.Body.Write(buffer, 0, buffer.Length);
            }
            
            return new ContentResult();
        }
    }
}