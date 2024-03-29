﻿using System;
using System.Threading;
using System.Threading.Tasks;

using RestWebservice_RemoteCompiling.Command;
using RestWebservice_RemoteCompiling.Database;
using RestWebservice_RemoteCompiling.Entities;
using RestWebservice_RemoteCompiling.Helpers;
using RestWebservice_RemoteCompiling.Repositories;

using Serilog;

namespace RestWebservice_RemoteCompiling.Handlers
{
    public class LoginHandler : BaseHandler<LoginCommand, CustomResponse<string>>
    {
        private readonly ILdapHelper _ldapHelper;
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        public LoginHandler(ILdapHelper ldapHelper, IUserRepository userRepository, ITokenService tokenService)
            : base(userRepository)
        {
            _ldapHelper = ldapHelper;
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public override async Task<CustomResponse<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                LdapUser? user = _ldapHelper.LogInUser(request.Username, request.Password);

                if (user is null)
                    return CustomResponse.Error<string>(401, "Invalid Credentials");

                User? dbUser = await _userRepository.GetUserByLdapUid(user.Uid);

                if (dbUser is not null)
                    return CustomResponse.Success(_tokenService.BuildToken(dbUser));

                dbUser = new User
                         {
                             Email = user.Mail,
                             Name = user.Cn,
                             LdapUid = user.Uid,
                             UserRole = user.Ou.Contains("Teacher") ? UserRole.Teacher : UserRole.DefaultUser
                         };

                dbUser = await _userRepository.AddUser(dbUser);

                return CustomResponse.Success(_tokenService.BuildToken(dbUser));
            }
            catch (Exception e)
            {
                Log.Error(e, $"{e.Message} \n\n{e.StackTrace}");

                return CustomResponse.Error<string>(500, "Unexpected Error");
            }
        }
    }
}