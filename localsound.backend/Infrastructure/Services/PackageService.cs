using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Domain.Model.Dto.Submission;
using localsound.backend.Domain.Model.Entity;
using localsound.backend.Infrastructure.Interface.Repositories;
using localsound.backend.Infrastructure.Interface.Services;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace localsound.backend.Infrastructure.Services
{
    public class PackageService : IPackageService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly ILogger<PackageService> _logger;

        public PackageService(IPackageRepository packageRepository, ILogger<PackageService> logger, IAccountRepository accountRepository)
        {
            _packageRepository = packageRepository;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<ServiceResponse> CreateArtistPackage(Guid appUserId, string memberId, CreatePackageDto packageDto)
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

                //var formData = packageDto.Photos.ToDictionary(o => o.Key, o => o.Value);

                //// upload photos if they exist
                //if (packageDto.Photos != null && packageDto.Photos.Any())
                //{
                //    var result = await _blobRepository
                //}

                var equipmentList = JsonSerializer.Deserialize<List<EquipmentDto>>(packageDto.PackageEquipment);

                // create artist package in tables
                var artistPackage = new ArtistPackage
                {
                    AppUserId = appUserId,
                    PackageName = packageDto.PackageName,
                    PackageDescription = packageDto.PackageDescription,
                    PackagePrice = packageDto.PackagePrice,
                    Equipment = equipmentList.Select(x => new ArtistPackageEquipment
                    {
                        EquipmentName = x.EquipmentName,
                    }).ToList()
                };

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
    }
}
