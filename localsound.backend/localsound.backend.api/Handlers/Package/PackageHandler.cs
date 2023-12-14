using localsound.backend.api.Commands.Packages;
using localsound.backend.Domain.Model;
using localsound.backend.Infrastructure.Interface.Services;
using MediatR;

namespace localsound.backend.api.Handlers.Package
{
    public class PackageHandler : IRequestHandler<CreateArtistPackageCommand, ServiceResponse>
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
    }
}
