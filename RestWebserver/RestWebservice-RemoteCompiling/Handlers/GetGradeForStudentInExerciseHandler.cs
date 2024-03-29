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
    public class GetGradeForStudentInExerciseHandler : BaseHandler<GetGradeForStudentInExerciseQuery, CustomResponse<ExerciseGradeEntity>>
    {
        private readonly IExerciseGradeRepository _exerciseGradeRepository;
        private readonly IUserRepository _userRepository;

        public GetGradeForStudentInExerciseHandler(IUserRepository userRepository, IExerciseGradeRepository exerciseGradeRepository)
            : base(userRepository)
        {
            _userRepository = userRepository;
            _exerciseGradeRepository = exerciseGradeRepository;
        }

        public override async Task<CustomResponse<ExerciseGradeEntity>> Handle(GetGradeForStudentInExerciseQuery request, CancellationToken cancellationToken)
        {
            string ldapIdent = request.Token.Claims.First(x => x.Type == ClaimTypes.Sid).Value;
            User? ldapUser = await _userRepository.GetUserByLdapUid(ldapIdent);

            if (ldapUser is null)
            {
                return CustomResponse.Error<ExerciseGradeEntity>(403);
            }

            ExerciseGrade? exerciseGrade = await _exerciseGradeRepository.Get(request.StudentId, request.ExerciseId);

            if (exerciseGrade is null || exerciseGrade.Status is GradingStatus.NotGraded or GradingStatus.InProcess)
            {
                return CustomResponse.Error<ExerciseGradeEntity>(404, "Grade not ready or not found");
            }

            UserEntity userEntity = new()
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

            var z = new ExerciseGradeEntity
                    {
                        Id = exerciseGrade.Id,
                        UserToGrade = userEntity,
                        Grade = exerciseGrade.Grade,
                        Status = exerciseGrade.Status,
                        Feedback = exerciseGrade.Feedback,
                        Project = project
                    };


            return CustomResponse.Success(z);
        }
    }
}