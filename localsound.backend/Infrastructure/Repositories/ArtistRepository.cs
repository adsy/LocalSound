﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<ArtistRepository> _logger;

        public ArtistRepository(LocalSoundDbContext dbContext, ILogger<ArtistRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> UpdateArtistFollowerAsync(Account follower, string artistId, bool startFollowing)
        {
            var followString = startFollowing ? "following" : "unfollowing";
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.MemberId == artistId);

                if (artist is null)
                {
                    return new ServiceResponse(HttpStatusCode.NotFound, $"There was an error while {followString} the artist, please try again.");
                }

                if (startFollowing)
                {
                    var artistFollower = await _dbContext.ArtistFollower.FirstOrDefaultAsync(x => x.ArtistId == artist.AppUserId && x.FollowerId == follower.AppUserId);

                    if (artistFollower != null)
                    {
                        return new ServiceResponse(HttpStatusCode.InternalServerError, $"There was an error while {followString} the artist, please try again.");
                    }

                    await _dbContext.ArtistFollower.AddAsync(new ArtistFollower
                    {
                        ArtistId = artist.AppUserId,
                        FollowerId = follower.AppUserId
                    });
                }
                else
                {
                    var artistFollower = await _dbContext.ArtistFollower.FirstOrDefaultAsync(x => x.ArtistId == artist.AppUserId && x.FollowerId == follower.AppUserId);

                    if (artistFollower is null)
                    {
                        return new ServiceResponse(HttpStatusCode.NotFound, $"There was an error while {followString} the artist, please try again.");
                    }

                    _dbContext.ArtistFollower.Remove(artistFollower);
                }

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistFollowerAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, $"There was an error while {followString} the artist, please try again.");
            }
        }

        public async Task<ServiceResponse> UpdateArtistPersonalDetails(Guid userId, UpdateArtistPersonalDetailsDto updateArtistDto)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Genres)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId);

                // Artist should not be null here, otherwise its an issue with the artist creation/DB
                if (artist is null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistPersonalDetails)} - Could not find matching artist with userId: {userId}";
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
                }

                artist.UpdateName(updateArtistDto.Name)
                    .UpdateAddress(updateArtistDto.Address)
                    .UpdatePhoneNumber(updateArtistDto.PhoneNumber)
                    .UpdateProfileUrl(updateArtistDto.ProfileUrl)
                    .UpdateSocialLinks(updateArtistDto.SoundcloudUrl, updateArtistDto.SpotifyUrl, updateArtistDto.YoutubeUrl)
                    .UpdateAboutSection(updateArtistDto.AboutSection);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistPersonalDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }

        public async Task<ServiceResponse> UpdateArtistProfileDetails(Guid userId, UpdateArtistProfileDetailsDto updateArtistDto)
        {
            try
            {
                var artist = await _dbContext.Account
                    .Include(x => x.Genres)
                    .Include(x => x.EventTypes)
                    .Include(x => x.Equipment)
                    .FirstOrDefaultAsync(x => x.AppUserId == userId);

                // Artist should not be null here, otherwise its an issue with the artist creation/DB
                if (artist is null)
                {
                    var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistProfileDetails)} - Could not find matching artist with userId: {userId}";
                    return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
                }

                var artistGenres = updateArtistDto.Genres.Select(x => new ArtistGenre
                {
                    AppUserId = artist.AppUserId,
                    GenreId = x.GenreId
                }).ToList();

                var eventTypes = updateArtistDto.EventTypes.Select(x => new ArtistEventType
                {
                    AppUserId = artist.AppUserId,
                    EventTypeId = x.EventTypeId
                }).ToList();

                var equipment = updateArtistDto.Equipment.Select(x => new ArtistEquipment
                {
                    AppUserId = artist.AppUserId,
                    EquipmentId = x.EquipmentId,
                    EquipmentName = x.EquipmentName
                }).ToList();

                artist.UpdateGenres(artistGenres)
                    .UpdateEventTypes(eventTypes)
                    .UpdateEquipment(equipment);

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(AccountRepository)} - {nameof(UpdateArtistProfileDetails)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError, "There was an error while updating your details, please try again.");
            }
        }
    }
}
