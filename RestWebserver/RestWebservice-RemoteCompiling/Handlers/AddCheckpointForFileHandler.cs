﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using RestWebservice_RemoteCompiling.Command;
using RestWebservice_RemoteCompiling.Database;
using RestWebservice_RemoteCompiling.Entities;
using RestWebservice_RemoteCompiling.Repositories;

namespace RestWebservice_RemoteCompiling.Handlers
{
    public class AddCheckpointForFileHandler : IRequestHandler<AddCheckpointForFileCommand, CustomResponse<int>>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserRepository _userRepository;
        public AddCheckpointForFileHandler(IUserRepository userRepository, IExerciseRepository exerciseRepository)
        {
            _userRepository = userRepository;
            _exerciseRepository = exerciseRepository;
        }
        public async Task<CustomResponse<int>> Handle(AddCheckpointForFileCommand request, CancellationToken cancellationToken)
        {
            string ldapIdent = request.Token.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
            User? ldapUser = _userRepository.GetUserByLdapUid(ldapIdent);

            if (!ldapUser.Projects.Any(x => x.Id == request.FileId))
                return CustomResponse.Error<int>(404, "File not found");

            foreach (Project ldapUserProject in ldapUser.Projects)
            {
                ldapUserProject.Files.ForEach(x =>
                                              {

                                                  if (x.Id == request.FileId)
                                                  {
                                                      x.LastModified = DateTime.Now;
                                                      x.Checkpoints.Add(request.Checkpoint);
                                                  }
                                              });
            }
            
            _userRepository.UpdateUser(ldapUser);

            return CustomResponse.Success(request.Checkpoint.Id);
        }
    }
}