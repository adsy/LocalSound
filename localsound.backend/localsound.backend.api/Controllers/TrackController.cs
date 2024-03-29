﻿using Azure;
using Azure.Storage.Blobs.Models;
using localsound.backend.api.Commands.Track;
using localsound.backend.api.Queries.Track;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.ModelAdaptor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace localsound.backend.api.Controllers
{
    [Route("api/track")]
    [ApiController]
    public class TrackController : BaseApiController
    {
        private readonly BlobStorageSettingsAdaptor _blobStorageSettings;

        public TrackController(BlobStorageSettingsAdaptor blobStorageSettings)
        {
            _blobStorageSettings = blobStorageSettings;
        }

        [HttpGet]
        [Route("member/{memberId}/upload-token")]
        public async Task<ActionResult> SASToken(string memberId)
        {
            var result = await Mediator.Send(new GetUploadTrackDataQuery
            {
                AppUserId = CurrentUser.AppUserId,
                MemberId = memberId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpPost]
        [Route("member/{memberId}/upload-track")]
        public async Task<ActionResult> UploadTrackSupportingDetails([FromForm] TrackUploadDto data, string memberId)
        {
            var result = await Mediator.Send(new AddTrackSupportingDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackData = data
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpPut]
        [Route("member/{memberId}/track/{trackId}")]
        public async Task<ActionResult> UpdateTrackSupportingDetails([FromForm] TrackUpdateDto data, string memberId, int trackId)
        {
            var result = await Mediator.Send(new UpdateTrackSupportingDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = trackId,
                TrackData = data
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpGet]
        [Route("member/{memberId}/track/{trackId}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetArtistTrack(string memberId, int trackId)
        {
            var result = await Mediator.Send(new GetArtistTrackQuery
            {
                MemberId = memberId,
                TrackId = trackId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }

        [HttpDelete]
        [Route("member/{memberId}/track/{trackId}")]
        public async Task<ActionResult> DeleteArtistTrack(string memberId, int trackId)
        {
            var result = await Mediator.Send(new DeleteArtistTrackCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = trackId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpPut]
        [Route("member/{memberId}/likes")]
        public async Task<ActionResult> LikeArtistTrack([FromBody]TrackLikeDto data, string memberId)
        {
            var result = await Mediator.Send(new LikeArtistTrackCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                TrackId = data.TrackId,
                ArtistMemberId = data.ArtistId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpDelete]
        [Route("member/{memberId}/likes/{songLikeId}")]
        public async Task<ActionResult> UnlikeArtistTrack(string memberId, int songLikeId)
        {
            var result = await Mediator.Send(new UnlikeArtistTrackCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                SongLikeId = songLikeId
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok();
        }

        [HttpGet]
        [Route("member/{memberId}/playlist-type/{playlistType}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetTracks([FromQuery] int? lastTrackId, string memberId, PlaylistTypeEnum playlistType)
        {
            var result = await Mediator.Send(new GetTracksQuery
            {
                UserId = CurrentUser?.AppUserId,
                MemberId = memberId,
                LastTrackId = lastTrackId,
                PlaylistType = playlistType
            });

            if (!result.IsSuccessStatusCode)
            {
                return StatusCode((int)result.StatusCode, result.ServiceResponseMessage);
            }

            return Ok(result.ReturnData);
        }
    }
}
