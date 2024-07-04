using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CitiesInformations.Api.Controllers
{
    [Route("api/files")]
    [Authorize]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
            ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet("{fileid}")]
        [ApiVersion(0.1, Deprecated =true)]
        public ActionResult GetFile(string fileid)
        {
            // look up the actual file, depending on the filed 

            //demo code

            var pathToFile = "Entity_Framework_Core_in_Action.pdf";

            // check if the file exist

            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_fileExtensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType, Path.GetFileName(pathToFile));
        }


        [HttpPost]

        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            // First Validate The Input

            if (file.Length == 0 || file.Length > 20974520
                || file.ContentType != "application/pdf")
            {

                return BadRequest("No File or Invalid has been inputted");

            }

            // create the file path 

            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf");


            // initialize file stream 

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("your file has been uploaded successfully");
        }

    }
}
