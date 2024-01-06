using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly LocalSoundDbContext _dbContext;
        private readonly ILogger<BookingRepository> _logger;

        public BookingRepository(LocalSoundDbContext dbContext, ILogger<BookingRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<ServiceResponse> CreateBookingAsync(Guid appUserId, CreateBookingDto bookingDto)
        {
            try
            {
                var artist = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.MemberId == bookingDto.ArtistId);

                if (artist == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured creating your booking, please try again..."
                    };
                }

                await _dbContext.ArtistBooking.AddAsync(new ArtistBooking
                {
                    ArtistId = artist.Id,
                    BookerId = appUserId,
                    PackageId = bookingDto.PackageId,
                    EventTypeId = bookingDto.EventTypeId,
                    BookingDescription = bookingDto.BookingDescription,
                    BookingAddress = bookingDto.BookingAddress,
                    BookingLength = bookingDto.BookingLength,
                    BookingDate = bookingDto.BookingDate,
                    BookingConfirmed = null
                });

                await _dbContext.SaveChangesAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(BookingRepository)} - {nameof(CreateBookingAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured creating your booking, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<List<ArtistBooking>>> GetFutureBookingsAsync(Guid appUserId, bool? bookingConfirmed, int page)
        {
            try
            {
                var user = await _dbContext.AppUser.FirstOrDefaultAsync(x => x.Id == appUserId);

                if (user == null)
                {
                    return new ServiceResponse<List<ArtistBooking>>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                var bookings =  await _dbContext.ArtistBooking.Where(x => x.BookerId == appUserId && x.BookingConfirmed == bookingConfirmed && x.BookingDate >= new DateTime())
                        .Include(x => x.Artist)
                        .Include(x => x.Booker)
                        .ThenInclude(x => x.NonArtist)
                        .Include(x => x.Package)
                        .ThenInclude(x => x.Equipment)
                        .Include(x => x.EventType)
                        .Skip(page * 10)
                        .Take(10)
                        .ToListAsync();

                return new ServiceResponse<List<ArtistBooking>>(HttpStatusCode.OK)
                {
                    ReturnData = bookings
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(BookingRepository)} - {nameof(GetFutureBookingsAsync)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistBooking>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                };
            }
        }
    }
}
