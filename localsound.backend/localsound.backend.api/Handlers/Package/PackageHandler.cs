using localsound.backend.api.Commands.Packages;
using localsound.backend.api.Queries.Packages;
using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Entity;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Package
{
    public class PackageHandler : IRequestHandler<CreateArtistPackageCommand, ServiceResponse>,
        IRequestHandler<GetArtistPackagesQuery, ServiceResponse<List<ArtistPackageDto>>>,
        IRequestHandler<DeleteArtistPackageCommand, ServiceResponse>
    {
        private readonly IPackageService _packageService;

        public PackageHandler(IPackageService packageService)
        {
            _packageService = packageService;
        }

        public async Task<ServiceResponse> Handle(CreateArtistPackageCommand request, CancellationToken cancellationToken)
        {
            return await _packageService.CreateArtistPackage(request.AppUserId, request.MemberId, request.PackageDto);
        }

        public async Task<ServiceResponse<List<ArtistPackageDto>>> Handle(GetArtistPackagesQuery request, CancellationToken cancellationToken)
        {
            return await _packageService.GetArtistPackages(request.MemberId);
        }

        public async Task<ServiceResponse> Handle(DeleteArtistPackageCommand request, CancellationToken cancellationToken)
        {
            return await _packageService.DeleteArtistPackage(request.AppUserId, request.MemberId, request.PackageId);
        }
    }
}
