﻿using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
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

        public async Task<ServiceResponse> AcceptBooking(Guid appUserId, string memberId, int bookingId)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured creating your booking, please try again..."
                    };
                }

                var acceptResult = await _bookingRepository.AcceptBookingAsync(appUserId, bookingId);

                if (!acceptResult.IsSuccessStatusCode)
                {
                    return acceptResult;
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(BookingService)} - {nameof(AcceptBooking)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured accepting your booking, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> CancelBooking(Guid appUserId, string memberId, int bookingId)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured cancelling your booking, please try again..."
                    };
                }

                var cancelResult = await _bookingRepository.CancelBookingAsync(appUserId, bookingId, appUser.ReturnData.CustomerType);

                if (!cancelResult.IsSuccessStatusCode)
                {
                    return cancelResult;
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                var message = $"{nameof(BookingService)} - {nameof(CancelBooking)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured cancelling your booking, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> CreateBooking(Guid appUserId, string memberId, CreateBookingDto bookingDto, CancellationToken cancellationToken)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
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

        public async Task<ServiceResponse<BookingListResponse>> GetCompletedBookings(Guid appUserId, string memberId, int? lastBookingId)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse<BookingListResponse>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                var userBookingsResult = await _bookingRepository.GetCompletedBookingsAsync(appUserId, lastBookingId);

                if (!userBookingsResult.IsSuccessStatusCode || userBookingsResult.ReturnData is null)
                {
                    return new ServiceResponse<BookingListResponse>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                var returnData = userBookingsResult.ReturnData.Select(x => new BookingDto
                {
                    BookingId = x.BookingId,
                    BookerId = x.Booker.MemberId,
                    BookerName = $"{x.Booker.FirstName} {x.Booker.LastName}",
                    ArtistId = appUser.ReturnData.CustomerType == CustomerTypeEnum.Artist ? memberId : x.Booker.MemberId,
                    ArtistName = x.Artist.Name,
                    PackageName = x.Package.PackageName,
                    PackagePrice = x.Package.PackagePrice,
                    BookingDate = x.BookingDate,
                    BookingLength = x.BookingLength,
                    EventType = x.EventType.EventTypeName,
                    BookingDescription = x.BookingDescription,
                    BookingAddress = x.BookingAddress,
                    BookingConfirmed = x.BookingConfirmed,
                    PackageEquipment = x.Package.Equipment.Select(equipment => new EquipmentDto
                    {
                        EquipmentId = equipment.ArtistPackageEquipmentId,
                        EquipmentName = equipment.EquipmentName,
                    }).ToList()
                }).ToList();

                return new ServiceResponse<BookingListResponse>(HttpStatusCode.OK)
                {
                    ReturnData = new BookingListResponse
                    {
                        Bookings = returnData,
                        CanLoadMore = returnData.Count == 10
                    }
                };
            }
            catch (Exception e)
            {
                var message = $"{nameof(BookingService)} - {nameof(GetCompletedBookings)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<BookingListResponse>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<BookingListResponse>> GetNonCompletedBookings(Guid appUserId, string memberId, bool? bookingConfirmed, int? lastBookingId)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse<BookingListResponse>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                var userBookingsResult = await _bookingRepository.GetNonCompletedBookingsAsync(appUserId, bookingConfirmed, lastBookingId);

                if (!userBookingsResult.IsSuccessStatusCode || userBookingsResult.ReturnData is null)
                {
                    return new ServiceResponse<BookingListResponse>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                    };
                }

                var returnData = userBookingsResult.ReturnData.Select(x => new BookingDto
                {
                    BookingId = x.BookingId,
                    BookerId = x.Booker.MemberId,
                    BookerName = $"{x.Booker.FirstName} {x.Booker.LastName}",
                    ArtistId = appUser.ReturnData.CustomerType == CustomerTypeEnum.Artist ? memberId : x.Booker.MemberId,
                    ArtistName = x.Artist.Name,
                    PackageName = x.Package.PackageName,
                    PackagePrice = x.Package.PackagePrice,
                    BookingDate = x.BookingDate,
                    BookingLength = x.BookingLength,
                    EventType = x.EventType.EventTypeName,
                    BookingDescription = x.BookingDescription,
                    BookingAddress = x.BookingAddress,
                    BookingConfirmed = x.BookingConfirmed,
                    PackageEquipment = x.Package.Equipment.Select(equipment => new EquipmentDto
                    {
                        EquipmentId = equipment.ArtistPackageEquipmentId,
                        EquipmentName = equipment.EquipmentName,
                    }).ToList()
                }).ToList();

                return new ServiceResponse<BookingListResponse>(HttpStatusCode.OK)
                {
                    ReturnData = new BookingListResponse
                    {
                        Bookings = returnData,
                        CanLoadMore = returnData.Count == 10
                    }
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(BookingService)} - {nameof(GetNonCompletedBookings)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<BookingListResponse>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting your bookings, please try again..."
                };
            }
        }
    }
}
