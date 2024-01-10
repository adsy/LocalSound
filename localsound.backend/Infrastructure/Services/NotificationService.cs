using AutoMapper;
using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ILogger<NotificationService> _logger;
        private readonly IMapper _mapper;

        public NotificationService(INotificationRepository notificationRepository, ILogger<NotificationService> logger, IMapper mapper, IAccountRepository accountRepository)
        {
            _notificationRepository = notificationRepository;
            _logger = logger;
            _mapper = mapper;
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResponse<NotificationCreatedResponseDto>> CreateNotification(Guid creatorUserId, string receiverMemberId, string message, string redirectUrl)
        {
            try
            {
                var receiver = await _accountRepository.GetAppUserFromDbAsync(receiverMemberId);

                if (!receiver.IsSuccessStatusCode || receiver.ReturnData == null)
                {
                    var itemAddErrorMessage = $"{nameof(NotificationService)} - {nameof(CreateNotification)} - " +
                        $"Error occured creating booking created notification for member:{receiverMemberId}";
                    _logger.LogError(itemAddErrorMessage);
                    return new ServiceResponse<NotificationCreatedResponseDto>(HttpStatusCode.InternalServerError);
                }

                var notification = await _notificationRepository.CreateNotificationAsync(creatorUserId, receiver.ReturnData.Id, message, redirectUrl);

                if (!notification.IsSuccessStatusCode || notification.ReturnData == null)
                {
                    var itemAddErrorMessage = $"{nameof(NotificationService)} - {nameof(CreateNotification)} - " +
                        $"Error occured creating booking created notification for member:{receiverMemberId}";
                    _logger.LogError(itemAddErrorMessage);

                    return new ServiceResponse<NotificationCreatedResponseDto>(HttpStatusCode.InternalServerError);
                }

                return new ServiceResponse<NotificationCreatedResponseDto>(HttpStatusCode.OK)
                {
                    ReturnData = new NotificationCreatedResponseDto
                    {
                        ReceiverUserId = receiver.ReturnData.Id,
                        Notification = new NotificationDto
                        {
                            NotificationId = notification.ReturnData.NotificationId,
                            ReceivingMemberId = notification.ReturnData.NotificationReceiver.MemberId,
                            CreatorMemberId = notification.ReturnData.NotificationCreator.MemberId,
                            NotificationMessage = notification.ReturnData.NotificationMessage,
                            RedirectUrl = notification.ReturnData.RedirectUrl,
                            NotificationViewed = notification.ReturnData.NotificationViewed,
                            UserImage = notification.ReturnData.NotificationCreator.Images.ToList().Any() ? notification.ReturnData.NotificationCreator.Images.ToList().Find(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage)?.AccountImageUrl : null
                        }
                    }
                };
            }
            catch (Exception e)
            {
                var errorMessage = $"{nameof(NotificationService)} - {nameof(CreateNotification)} - {e.Message}";
                _logger.LogError(e, errorMessage);

                return new ServiceResponse<NotificationCreatedResponseDto>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = $"Error occured creating booking created notification for member:{receiverMemberId}"
                };
            }
        }

        public async Task<ServiceResponse<List<NotificationDto>>> GetUserNotifications(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var notifications = await _notificationRepository.GetUserNotificationsAsync(userId, 0);

                if (!notifications.IsSuccessStatusCode || notifications.ReturnData == null)
                {
                    var itemAddErrorMessage = $"{nameof(NotificationService)} - {nameof(GetUserNotifications)} - " +
                        $"Error occured retrieving notifications for user:{userId}";
                    _logger.LogError(itemAddErrorMessage);

                    return new ServiceResponse<List<NotificationDto>>(HttpStatusCode.InternalServerError);
                }

                var notificationList = notifications.ReturnData.Select(x => new NotificationDto
                {
                    NotificationId = x.NotificationId, 
                    ReceivingMemberId = x.NotificationReceiver.MemberId,
                    CreatorMemberId = x.NotificationCreator.MemberId, 
                    NotificationMessage = x.NotificationMessage,
                    RedirectUrl = x.RedirectUrl,
                    NotificationViewed = x.NotificationViewed,
                    UserImage = x.NotificationCreator.Images.ToList().Any() ? x.NotificationCreator.Images.ToList().Find(x => x.AccountImageTypeId == AccountImageTypeEnum.ProfileImage)?.AccountImageUrl : null
                }).ToList();

                return new ServiceResponse<List<NotificationDto>>(HttpStatusCode.OK)
                {
                    ReturnData = notificationList
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(NotificationService)} - {nameof(GetUserNotifications)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<NotificationDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting user notifications"
                };
            }
        }
    }
}
