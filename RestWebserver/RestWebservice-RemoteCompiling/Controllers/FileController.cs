﻿using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using RestWebservice_RemoteCompiling.Command;
using RestWebservice_RemoteCompiling.Entities;
using RestWebservice_RemoteCompiling.Extensions;
using RestWebservice_RemoteCompiling.Helpers;

namespace RestWebservice_RemoteCompiling.Controllers
{
    [Route("api/file")]
    [ApiController]
    [EnableCors("AllAllowedPolicy")]
    [Authorize]
    public class FileController : BaseController
    {
        private readonly IMediator _mediator;
        public FileController(ITokenService tokenService, IMediator mediator)
            : base(tokenService)
        {
            _mediator = mediator;
        }
        [HttpPost("add")]
        public async Task<IActionResult> AddFileForProject(AddFileForProjectCommand command)
        {
            command.Token = GetTokenFromAuthorization();

            CustomResponse<int> result = await _mediator.Send(command);

            return result.ToResponse();
        }


        [HttpPost("addCheckPoint")]
        public async Task<IActionResult> AddCheckpointForFile(AddCheckpointForFileCommand command)
        {
            command.Token = GetTokenFromAuthorization();

            CustomResponse<int> result = await _mediator.Send(command);
            return result.ToResponse();
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateFileForUser(UpdateFileForProjectCommand command)
        {
            command.Token = GetTokenFromAuthorization();

            CustomResponse<bool> result = await _mediator.Send(command);

            return result.ToResponse();
        }


        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFileForProject(RemoveFileForProjectCommand command)
        {
            command.Token = GetTokenFromAuthorization();

            CustomResponse<bool> response = await _mediator.Send(command);

            return response.ToResponse();
        }
    }
}