using Microsoft.AspNetCore.Http;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class PhotoUploadDto
    {
        public Guid PhotoId { get; set; }
        public IFormFile Image { get; set; }
    }
}
