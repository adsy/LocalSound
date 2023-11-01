using localsound.backend.Domain.Model.Dto.Entity;
using System;
namespace localsound.backend.Domain.Model.Dto.Response
{
    public class TrackListResponseDto
    {
        public List<ArtistTrackUploadDto> TrackList { get; set; }
        public bool CanLoadMore { get; set; }
    }
}
