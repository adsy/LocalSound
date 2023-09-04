using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Dto.Entity
{
    public class AccountImageDto
    {
        public AccountImageTypeEnum AccountImageTypeId { get; set; }
        public string AccountImageUrl { get; set; }
    }
}
