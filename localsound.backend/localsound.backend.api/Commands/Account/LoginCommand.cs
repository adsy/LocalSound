﻿using localsound.backend.Domain.Model;
using localsound.backend.Domain.Model.Dto.Response;
using localsound.backend.Domain.Model.Dto.Submission;
using MediatR;

namespace localsound.backend.api.Commands.Account
{
    public class LoginCommand: IRequest<ServiceResponse<LoginResponseDto>>
    {
        public LoginSubmissionDto LoginDetails { get; set; }
    }
}
