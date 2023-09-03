using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class FileUploadDto
    {
        public string FileName { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
