﻿using System.Collections.Generic;

using RestWebservice_RemoteCompiling.Database;

namespace RestWebservice_RemoteCompiling.Repositories
{
    public interface IExerciseGradeRepository
    {
        public int Add(ExerciseGrade exerciseGrade);

        public List<ExerciseGrade> Get(string studentId);

        public ExerciseGrade? Get(string studentId, int exerciseId);

        public List<ExerciseGrade> Get(int exerciseId);
    }
}