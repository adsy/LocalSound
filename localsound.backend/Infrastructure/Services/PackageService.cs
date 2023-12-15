using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace localsound.backend.Infrastructure.Services
{
    public class PackageService : IPackageService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly ILogger<PackageService> _logger;

        public PackageService(IPackageRepository packageRepository, ILogger<PackageService> logger, IAccountRepository accountRepository, IBlobRepository blobRepository)
        {
            _packageRepository = packageRepository;
            _logger = logger;
            _accountRepository = accountRepository;
            _blobRepository = blobRepository;
        }

        public async Task<ServiceResponse> CreateArtistPackage(Guid appUserId, string memberId, ArtistPackageSubmissionDto packageDto)
        {
            try
            {
                var appUser = await _accountRepository.GetAppUserFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
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
                    Equipment = artistPackageEquipment
                };

                var photoIds = JsonConvert.DeserializeObject<List<Guid>>(packageDto.PhotoIds);
                var photos = new List<PhotoUploadDto>();

                for(int i = 0; i < packageDto.Photos?.Count; i++)
                {
                    photos.Add(new PhotoUploadDto
                    {
                        PhotoId = photoIds[i],
                        Image = packageDto.Photos[i]
                    });
                }

                foreach (var photo in photos)
                {
                    var packagePhoto = new ArtistPackagePhoto();
                    var ext = ".png";
                    var fileLocation = $"[{appUserId}]/packages/{packageId}/photos/{photo.PhotoId}{ext}";
                    var photoUploadResult = await _blobRepository.UploadBlobAsync(fileLocation, photo.Image);

                    if (!photoUploadResult.IsSuccessStatusCode || photoUploadResult.ReturnData == null)
                    {
                        return new ServiceResponse(HttpStatusCode.InternalServerError)
                        {
                            ServiceResponseMessage = "An error occured creating your package, please try again..."
                        };
                    }

                    packagePhoto.PhotoUrl = photoUploadResult.ReturnData;
                    packagePhoto.FileContent = new FileContent
                    {
                        FileLocation = fileLocation,
                        FileContentId = photo.PhotoId,
                        FileExtensionType = ext
                    };

                    artistPackage.PackagePhotos.Add(packagePhoto);
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
                var appUser = await _accountRepository.GetAppUserFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var packageResult = await _packageRepository.GetArtistPackageAsync(appUserId, packageId);

                if (!packageResult.IsSuccessStatusCode || packageResult.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                // delete images
                foreach (var photo in packageResult.ReturnData.PackagePhotos) 
                {
                    var photoDeleteResult = await _blobRepository.DeleteBlobAsync(photo.FileContent.FileLocation);

                    if (!photoDeleteResult.IsSuccessStatusCode)
                    {
                        //TODO: Push delete operation to queue if it fails
                    }
                }

                var deleteResult = await _packageRepository.DeleteArtistPackageAsync(packageResult.ReturnData);

                if (!deleteResult.IsSuccessStatusCode)
                {
                    return deleteResult;
                }

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

                if (!result.IsSuccessStatusCode || result.ReturnData == null)
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
                    Photos = x.PackagePhotos.Select(photos => new ArtistPackagePhotoDto
                    {
                        ArtistPackagePhotoId = photos.ArtistPackagePhotoId,
                        ArtistPackagePhotoUrl = photos.PhotoUrl
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
                var appUser = await _accountRepository.GetAppUserFromDbAsync(appUserId, memberId);

                if (!appUser.IsSuccessStatusCode || appUser.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var package = await _packageRepository.GetArtistPackageAsync(appUserId, packageId);

                if (!package.IsSuccessStatusCode || package.ReturnData == null)
                {
                    return new ServiceResponse(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured deleting your package, please try again..."
                    };
                }

                var deletedPhotos = new List<ArtistPackagePhoto>();
                // deleted images
                if (!string.IsNullOrWhiteSpace(packageDto.DeletedPhotoIds))
                {
                    var deletedIds = JsonConvert.DeserializeObject<List<Guid>>(packageDto.DeletedPhotoIds);
                    if (deletedIds != null && deletedIds.Any())
                    {
                        foreach (var id in deletedIds)
                        {
                            var packagePhoto = package.ReturnData.PackagePhotos.FirstOrDefault(x => x.ArtistPackagePhotoId == id);

                            var azureDeleteResult = await _blobRepository.DeleteBlobAsync(packagePhoto.FileContent.FileLocation);

                            if (!azureDeleteResult.IsSuccessStatusCode)
                            {
                                //TODO: Push this to a queue so it can be done later
                            }

                            deletedPhotos.Add(packagePhoto);
                        }
                    }
                }

                // new images
                var newPhotos = new List<ArtistPackagePhoto>();
                if (!string.IsNullOrWhiteSpace(packageDto.PhotoIds))
                {
                    var photoIds = JsonConvert.DeserializeObject<List<Guid>>(packageDto.PhotoIds);
                    var photos = new List<PhotoUploadDto>();

                    for (int i = 0; i < packageDto.Photos?.Count; i++)
                    {
                        photos.Add(new PhotoUploadDto
                        {
                            PhotoId = photoIds[i],
                            Image = packageDto.Photos[i]
                        });
                    }

                    foreach (var photo in photos)
                    {
                        var packagePhoto = new ArtistPackagePhoto();
                        var ext = ".png";
                        var fileLocation = $"[{appUserId}]/packages/{packageId}/photos/{photo.PhotoId}{ext}";
                        var photoUploadResult = await _blobRepository.UploadBlobAsync(fileLocation, photo.Image);

                        if (!photoUploadResult.IsSuccessStatusCode || photoUploadResult.ReturnData == null)
                        {
                            return new ServiceResponse(HttpStatusCode.InternalServerError)
                            {
                                ServiceResponseMessage = "An error occured creating your package, please try again..."
                            };
                        }

                        packagePhoto.PhotoUrl = photoUploadResult.ReturnData;
                        packagePhoto.FileContentId = photo.PhotoId;
                        packagePhoto.FileContent = new FileContent
                        {
                            FileLocation = fileLocation,
                            FileContentId = photo.PhotoId,
                            FileExtensionType = ext
                        };
                        newPhotos.Add(packagePhoto);
                    }
                }

                var equipmentList = JsonConvert.DeserializeObject<List<EquipmentDto>>(packageDto.PackageEquipment);
                var artistPackageEquipment = new List<ArtistPackageEquipment>();

                if (equipmentList != null && equipmentList.Any())
                {
                    artistPackageEquipment = equipmentList.Select(x => new ArtistPackageEquipment
                    {
                        ArtistPackageId = package.ReturnData.ArtistPackageId,
                        ArtistPackageEquipmentId = x.EquipmentId,
                        EquipmentName = x.EquipmentName,
                    }).ToList();
                }

                var equipmentUpdate = await _packageRepository.UpdateArtistPackageEquipmentAsync(package.ReturnData.ArtistPackageId, artistPackageEquipment);

                var updateResult = await _packageRepository.UpdateArtistPackageAsync(package.ReturnData.ArtistPackageId, packageDto.PackageName, packageDto.PackageDescription, packageDto.PackagePrice, newPhotos, deletedPhotos);

                if (!updateResult.IsSuccessStatusCode)
                {
                    return new ServiceResponse<List<ArtistPackageDto>>(HttpStatusCode.InternalServerError)
                    {
                        ServiceResponseMessage = "An error occured updating your package, please try again..."
                    };
                }

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
