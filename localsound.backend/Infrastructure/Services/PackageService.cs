using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using LocalSound.Shared.Package.ServiceBus.Dto;
using LocalSound.Shared.Package.ServiceBus.Dto.Enum;
using LocalSound.Shared.Package.ServiceBus.Dto.QueueMessage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class PackageService : IPackageService
    {
        private readonly IDbTransactionRepository _dbTransactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IServiceBusRepository _serviceBusRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly ILogger<PackageService> _logger;

        public PackageService(IPackageRepository packageRepository, ILogger<PackageService> logger, IAccountRepository accountRepository, IBlobRepository blobRepository, IServiceBusRepository serviceBusRepository, IDbTransactionRepository dbTransactionRepository)
        {
            _packageRepository = packageRepository;
            _logger = logger;
            _accountRepository = accountRepository;
            _blobRepository = blobRepository;
            _serviceBusRepository = serviceBusRepository;
            _dbTransactionRepository = dbTransactionRepository;
        }

        public async Task<ServiceResponse> CreateArtistPackage(Guid appUserId, string memberId, ArtistPackageSubmissionDto packageDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured creating your package, please try again..."
                    };
                }

                var equipmentList = JsonConvert.DeserializeObject<List<EquipmentDto>>(packageDto.PackageEquipment);
                var artistPackageEquipment = new List<ArtistPackageEquipment>();

                if (equipmentList != null && equipmentList.Any())
                {
                    artistPackageEquipment = equipmentList.Select(x => new ArtistPackageEquipment
                    {
                        ArtistPackageEquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName,
                    }).ToList();
                }

                var packageId = Guid.NewGuid();

                var artistPackage = new ArtistPackage
                {
                    ArtistPackageId = packageId,
                    AppUserId = appUserId,
                    PackageName = packageDto.PackageName,
                    PackageDescription = packageDto.PackageDescription,
                    PackagePrice = packageDto.PackagePrice,
                    Equipment = artistPackageEquipment,
                    IsAvailable = true,
                    PackagePhotos = new List<ArtistPackageImage>()
                };

                if (packageDto.Photos != null && packageDto.Photos.Any())
                {
                    foreach (var photo in packageDto.Photos)
                    {
                        var packagePhoto = new ArtistPackageImage();
                        var fileContentId = Guid.NewGuid();
                        var ext = ".png";
                        var fileLocation = $"[{appUserId}]/packages/{packageId}/photos/{fileContentId}{ext}";
                        var photoUploadResult = await _blobRepository.UploadBlobAsync(fileLocation, photo);

                        if (!photoUploadResult.IsSuccessStatusCode || photoUploadResult.ReturnData is null)
                        {
                            return new ServiceResponse(HttpStatusCode.InternalServerError)
                            {
                                ServiceResponseMessage = "An error occured creating your package, please try again..."
                            };
                        }

                        packagePhoto.PhotoUrl = photoUploadResult.ReturnData;
                        packagePhoto.ArtistPackageImageFileContent = new ArtistPackageImageFileContent
                        {
                            FileLocation = fileLocation,
                            FileContentId = fileContentId,
                            FileExtensionType = ext
                        };

                        artistPackage.PackagePhotos.Add(packagePhoto);
                    }

                }

                var result = await _packageRepository.CreateArtistPackageAsync(artistPackage);

                if (!result.IsSuccessStatusCode)
                {
                    return result;
                }

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageService)} - {nameof(CreateArtistPackage)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured creating your package, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> DeleteArtistPackage(Guid appUserId, string memberId, Guid packageId)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var packageResult = await _packageRepository.GetArtistPackageAsync(appUserId, packageId);

                if (!packageResult.IsSuccessStatusCode || packageResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var deleteResult = await _packageRepository.MarkPackageAsUnavailable(packageResult.ReturnData);

                if (!deleteResult.IsSuccessStatusCode)
                {
                    return deleteResult;
                }

                var serviceBusResult = await _serviceBusRepository.SendDeleteQueueEntry(new ServiceBusMessageDto<DeletePackagePhotosDto>
                {
                    Command = DeleteEntityTypeEnum.DeletePackagePhotos,
                    Data = new DeletePackagePhotosDto
                    {
                        UserId = appUserId,
                        PackageId = packageId,
                    }
                });

                if (!serviceBusResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }
                
                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageService)} - {nameof(DeleteArtistPackage)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured deleting your package, please try again..."
                };
            }
        }

        public async Task<ServiceResponse<List<ArtistPackageDto>>> GetArtistPackages(string memberId)
        {
            try
            {
                var result = await _packageRepository.GetArtistPackagesAsync(memberId);

                if (!result.IsSuccessStatusCode || result.ReturnData is null)
                {
                    return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.InternalServerError, result.ServiceResponseMessage);
                }

                var returnData = result.ReturnData.Select(x => new ArtistPackageDto
                {
                    ArtistPackageId = x.ArtistPackageId,
                    ArtistPackageName = x.PackageName,
                    ArtistPackageDescription = x.PackageDescription,
                    ArtistPackagePrice = x.PackagePrice,
                    Equipment = x.Equipment.Select(equipment => new EquipmentDto
                    {
                        EquipmentId = equipment.ArtistPackageEquipmentId,
                        EquipmentName = equipment.EquipmentName,
                    }).ToList(),
                    Photos = x.PackagePhotos.Select(photo => new ArtistPackagePhotoDto
                    {
                        ArtistPackagePhotoUrl = photo.PhotoUrl,
                        ArtistPackagePhotoId = photo.ArtistPackageImageId
                    }).ToList()
                }).ToList();

                return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.OK)
                {
                    ReturnData = returnData
                };
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageService)} - {nameof(GetArtistPackages)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured getting artist packages, please try again..."
                };
            }
        }

        public async Task<ServiceResponse> UpdateArtistPackage(Guid appUserId, string memberId, Guid packageId, ArtistPackageSubmissionDto packageDto)
        {
            try
            {
                await _dbTransactionRepository.BeginTransactionAsync();

                var appUser = await _accountRepository.GetAccountFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var equipmentList = JsonConvert.DeserializeObject<List<EquipmentDto>>(packageDto.PackageEquipment);
                var artistPackageEquipment = new List<ArtistPackageEquipment>();

                if (equipmentList != null && equipmentList.Any())
                {
                    artistPackageEquipment = equipmentList.Select(x => new ArtistPackageEquipment
                    {
                        ArtistPackageId = packageId,
                        ArtistPackageEquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName,
                    }).ToList();
                }

                var equipmentUpdate = await _packageRepository.UpdateArtistPackageEquipmentAsync(appUserId, packageId, artistPackageEquipment);

                var packageResult = await _packageRepository.GetArtistPackageAsync(appUserId, packageId);

                if (!packageResult.IsSuccessStatusCode || packageResult.ReturnData is null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var deletedPhotos = new List<ArtistPackageImage>();
                // deleted images
                if (!string.IsNullOrWhiteSpace(packageDto.DeletedPhotoIds))
                {
                    var deletedIds = JsonConvert.DeserializeObject<List<int>>(packageDto.DeletedPhotoIds);
                    if (deletedIds != null && deletedIds.Any())
                    {
                        var deletePhotosResult = await _packageRepository.MarkPhotosForDeletion(packageId, deletedIds);
                        
                        if (!deletePhotosResult.IsSuccessStatusCode)
                            return new ServiceResponse(HttpStatusCode.InternalServerError)
                            {
                                ServiceResponseMessage = "An error occured deleting your package, please try again..."
                            };

                        var serviceBusResult = await _serviceBusRepository.SendDeleteQueueEntry<ServiceBusMessageDto<DeletePackagePhotosDto>>(new ServiceBusMessageDto<DeletePackagePhotosDto>
                        {
                            Command = DeleteEntityTypeEnum.DeletePackagePhotos,
                            Data = new DeletePackagePhotosDto
                            {
                                UserId = appUserId, 
                                PackageId = packageId,
                            }
                        });

                        if (!serviceBusResult.IsSuccessStatusCode)
                            return new ServiceResponse(HttpStatusCode.InternalServerError)
                            {
                                ServiceResponseMessage = "An error occured deleting your package, please try again..."
                            };
                    }
                }

                // new images
                var newPhotos = new List<ArtistPackageImage>();

                if (packageDto.Photos != null && packageDto.Photos.Any())
                {
                    foreach (var photo in packageDto.Photos)
                    {
                        var packagePhoto = new ArtistPackageImage();
                        var fileContentId = Guid.NewGuid();
                        var ext = ".png";
                        var fileLocation = $"[{appUserId}]/packages/{packageId}/photos/{fileContentId}{ext}";
                        var photoUploadResult = await _blobRepository.UploadBlobAsync(fileLocation, photo);

                        if (!photoUploadResult.IsSuccessStatusCode || photoUploadResult.ReturnData is null)
                        {
                            return new ServiceResponse(HttpStatusCode.InternalServerError)
                            {
                                ServiceResponseMessage = "An error occured creating your package, please try again..."
                            };
                        }

                        packagePhoto.PhotoUrl = photoUploadResult.ReturnData;
                        packagePhoto.ArtistPackageImageFileContent = new ArtistPackageImageFileContent
                        {
                            FileLocation = fileLocation,
                            FileContentId = fileContentId,
                            FileExtensionType = ext
                        };
                        newPhotos.Add(packagePhoto);
                    }
                }

                var updateResult = await _packageRepository.UpdateArtistPackageAsync(packageResult.ReturnData.ArtistPackageId, packageDto.PackageName, packageDto.PackageDescription, packageDto.PackagePrice, newPhotos);

                if (!updateResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured updating your package, please try again..."
                    };
                }

                await _dbTransactionRepository.CommitTransactionAsync();

                return new ServiceResponse(HttpStatusCode.OK);
            }
            catch(Exception e)
            {
                var message = $"{nameof(PackageService)} - {nameof(UpdateArtistPackage)} - {e.Message}";
                _logger.LogError(e, message);

                return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.InternalServerError)
                {
                    ServiceResponseMessage = "An error occured updating your package, please try again..."
                };
            }
        }
    }
}
