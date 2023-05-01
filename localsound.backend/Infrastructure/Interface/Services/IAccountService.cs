using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto;

namespace localsound.backend.Infrastructure.Interface.Services
{
    public interface IAccountService
    {
        Task<ServiceResponse<LoginResponseDto>> LoginAsync(LoginDataDto loginData);
    }
}
