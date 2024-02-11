using localsound.backend.Domain.Model.Dto.Entity;

namespace localsound.backend.Domain.Model.Dto.Submission
{
    public class SaveOnboardingDataDto
    {
        public string? AboutSection { get; set; }
        public List<GenreDto> Genres { get; set; }
        public List<EquipmentDto>? Equipment { get; set; }
        public List<EventTypeDto>? EventTypes { get; set; }
    }
}
