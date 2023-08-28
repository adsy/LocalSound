using localsound.backend.Infrastructure.Interface.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace localsound.backend.api.Controllers
{
    public class BaseApiController: ControllerBase
    {
        private IMediator _mediator;
        private ICurrentUserService _currentUserService;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ICurrentUserService CurrentUser => _currentUserService ??= HttpContext.RequestServices.GetService<ICurrentUserService>();
    }
}
