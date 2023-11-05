﻿using localsound.backend.api.Commands.Account;
using localsound.backend.api.Commands.Artist;
using localsound.backend.Domain.Model.Dto.Submission;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    [Route("api/artist")]
    [ApiController]
    public class ArtistController : BaseApiController
    {
        [HttpPut]
        [Route("member/{memberId}/personal-details")]
        public async Task<ActionResult> UpdateArtistPersonalDetails([FromBody] UpdateArtistPersonalDetailsDto updateArtistDto, string memberId)
        {
            var result = await Mediator.Send(new UpdateArtistPersonalDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                UpdateArtistDto = updateArtistDto
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }

        [HttpPut]
        [Route("member/{memberId}/profile-details")]
        public async Task<ActionResult> UpdateArtistProfileDetails([FromBody] UpdateArtistProfileDetailsDto updateArtistDto, string memberId)
        {
            var result = await Mediator.Send(new UpdateArtistProfileDetailsCommand
            {
                UserId = CurrentUser.AppUserId,
                MemberId = memberId,
                UpdateArtistDto = updateArtistDto
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode);
        }

        [HttpPost]
        [Route("follow-artist/member/{followerId}/artist/{artistId}")]
        public async Task<ActionResult> FollowArtist(string followerId, string artistId)
        {
            var result = await Mediator.Send(new FollowArtistCommand
            {
                UserId = CurrentUser.AppUserId,
                ArtistId = artistId,
                MemberId = followerId
            });

            if (result.IsSuccessStatusCode)
            {
                return Ok();
            }

            return StatusCode((int)result.StatusCode, result);
        }

        // Search artists by genre

        // Search artists by name

        // Search artists by location
    }
}
