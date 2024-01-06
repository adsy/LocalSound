using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Interfaces.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class BookingService : IBookingService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(IBookingRepository bookingRepository, ILogger<BookingService> logger, IAccountRepository accountRepository)
        {
            _bookingRepository = bookingRepository;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResponse> CreateBooking(Guid appUserId, string memberId, CreateBookingDto bookingDto, CancellationToken cancellationToken)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured creating your booking, please try again..."
                    };
                }

                var createResult = await _bookingRepository.CreateBookingAsync(appUserId, bookingDto);

                if (!createResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured creating your booking, please try again..."
                    };
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(BookingService)} - {nameof(CreateBooking)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured creating your booking, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<List<BookingDto>>> GetFutureBookings(Guid appUserId, string memberId, bool? bookingConfirmed, int page)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse<List<BookingDto>>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                var userBookingsResult = await _bookingRepository.GetFutureBookingsAsync(appUserId, bookingConfirmed, page);

                if (!userBookingsResult.IsSuccessStatusCode || userBookingsResult.ReturnData == null)
                {
                    return new ServiceResponse<List<BookingDto>>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                // TODO: Convert repo result into summary dto
                var returnData = userBookingsResult.ReturnData.Select(x => new BookingDto
                {
                    BookingId = x.BookingId,
                    BookerId = x.Booker.MemberId,
                    BookerName = $"{x.Booker.NonArtist.FirstName} {x.Booker.NonArtist.LastName}",
                    ArtistId = appUser.ReturnData.CustomerType == CustomerTypeEnum.Artist ? memberId : x.Booker.MemberId,
                    ArtistName = x.Artist.Name,
                    PackageName = x.Package.PackageName,
                    PackagePrice = x.Package.PackagePrice,
                    BookingDate = x.BookingDate,
                    BookingLength = x.BookingLength,
                    EventType = x.EventType.EventTypeName,
                    BookingDescription = x.BookingDescription,
                    BookingAddress = x.BookingAddress,
                    PackageEquipment = x.Package.Equipment.Select(equipment => new EquipmentDto
                    {
                        EquipmentId = equipment.ArtistPackageEquipmentId,
                        EquipmentName = equipment.EquipmentName,
                    }).ToList()
                }).ToList();

                return new ServiceResponse<List<BookingDto>>(HttpStatusCode.OK)
                {
                    ReturnData = returnData
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(BookingService)} - {nameof(GetFutureBookings)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<BookingDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                };
            }
        }
    }
}
