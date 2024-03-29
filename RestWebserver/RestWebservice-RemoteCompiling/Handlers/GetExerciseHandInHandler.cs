﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

using RestWebservice_RemoteCompiling.Database;
using RestWebservice_RemoteCompiling.Entities;
using RestWebservice_RemoteCompiling.Query;
using RestWebservice_RemoteCompiling.Repositories;

namespace RestWebservice_RemoteCompiling.Handlers
{
    public class GetExerciseHandInHandler : BaseHandler<GetExerciseHandInQuery, CustomResponse<ExerciseEntity>>
    {
        private readonly IExerciseRepository _exerciseRepository;
        private readonly IUserRepository _userRepository;

        public GetExerciseHandInHandler(IExerciseRepository exerciseRepository, IUserRepository userRepository)
            : base(userRepository)
        {
            _exerciseRepository = exerciseRepository;
            _userRepository = userRepository;
        }

        public override async Task<CustomResponse<ExerciseEntity>> Handle(GetExerciseHandInQuery request, CancellationToken cancellationToken)
        {
            Exercise? dbExercise = await _exerciseRepository.Get(request.Id);

            if (dbExercise is null)
                return CustomResponse.Error<ExerciseEntity>(404, "Exercise not found");

            string ldapIdent = request.Token.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
            User? ldapUser = await _userRepository.GetUserByLdapUid(ldapIdent);

            if (ldapUser is null /*|| ldapUser.UserRole != UserRole.Teacher */)
            {
                return CustomResponse.Error<ExerciseEntity>(403);
            }

            List<ExerciseGradeEntity> exerciseGradeList = dbExercise.HandIns.ConvertAll(exerciseGrade =>
                                                                                        {
                                                                                            UserEntity user = new()
                                                                                                              {
                                                                                                                  Email = exerciseGrade.UserToGrade.Email,
                                                                                                                  Name = exerciseGrade.UserToGrade.Name,
                                                                                                                  LdapUid = exerciseGrade.UserToGrade.LdapUid,
                                                                                                                  UserRole = exerciseGrade.UserToGrade.UserRole
                                                                                                              };
                                                                                            ProjectEntity project = new()
                                                                                                                    {
                                                                                                                        ExerciseID = exerciseGrade.Exercise.Id,
                                                                                                                        Id = exerciseGrade.Project.Id,
                                                                                                                        ProjectName = exerciseGrade.Project.ProjectName,
                                                                                                                        ProjectType = exerciseGrade.Project.ProjectType,
                                                                                                                        StdIn = exerciseGrade.Project.StdIn
                                                                                                                    };
                                                                                            project.Files = exerciseGrade.Project.Files.ConvertAll(x =>
                                                                                                                                                   {
                                                                                                                                                       return new FileEntity
                                                                                                                                                              {
                                                                                                                                                                  FileName = x.FileName,
                                                                                                                                                                  Id = x.Id,
                                                                                                                                                                  LastModified = x.LastModified,
                                                                                                                                                                  Checkpoints = new List<CheckPointEntity>
                                                                                                                                                                                {
                                                                                                                                                                                    new()
                                                                                                                                                                                    {
                                                                                                                                                                                        Code = x.Checkpoint.Code,
                                                                                                                                                                                        Created = x.Checkpoint.Created,
                                                                                                                                                                                        Id = x.Checkpoint.Id
                                                                                                                                                                                    }
                                                                                                                                                                                }
                                                                                                                                                              };
                                                                                                                                                   });

                                                                                            return new ExerciseGradeEntity
                                                                                                   {
                                                                                                       Id = exerciseGrade.Id,
                                                                                                       UserToGrade = user,
                                                                                                       Grade = exerciseGrade.Grade,
                                                                                                       Status = exerciseGrade.Status,
                                                                                                       Feedback = exerciseGrade.Feedback,
                                                                                                       Project = project
                                                                                                   };
                                                                                        });
            ExerciseEntity x = new()
                               {
                                   Author = dbExercise.Author.LdapUid,
                                   Description = dbExercise.Description,
                                   Id = dbExercise.Id,
                                   Name = dbExercise.Name,
                                   Template = dbExercise.Template,
                                   HandIns = exerciseGradeList,
                                   DueDate = dbExercise.DueDate,
                                   TaskDefinition = dbExercise.TaskDefinition
                               };

            return CustomResponse.Success(x);
        }
    }
}