namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class ArtistPackageDto
    {
        public Guid ArtistPackageId { get; set; }
        public string ArtistPackageName { get; set; }
        public string ArtistPackageDescription { get; set; }
        public string ArtistPackagePrice { get; set; }

        public List<EquipmentDto> Equipment { get; set; }
        public List<ArtistPackagePhotoDto> Photos { get; set; }
    }
}
