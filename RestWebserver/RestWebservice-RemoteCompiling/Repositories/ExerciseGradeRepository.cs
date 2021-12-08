﻿using System.Collections.Generic;
using System.Linq;

using RestWebservice_RemoteCompiling.Database;

namespace RestWebservice_RemoteCompiling.Repositories
{
    public class ExerciseGradeRepository : IExerciseGradeRepository
    {
        private RemoteCompileDbContext _context;
        public ExerciseGradeRepository(RemoteCompileDbContext context)
        {
            _context = context;
        }

        public int Add(ExerciseGrade exerciseGrade)
        {
            _context.ExerciseGrades.Add(exerciseGrade);
            return _context.SaveChanges();
        }
        
        public List<ExerciseGrade> Get(string studentId)
        {
            return _context.ExerciseGrades.Where(x=> x.UserToGrade.LdapUid == studentId).ToList();
        }
        
        public ExerciseGrade? Get(string studentId,int exerciseId)
        {
            return _context.ExerciseGrades.FirstOrDefault(x => x.Exercise.Id == exerciseId && x.UserToGrade.LdapUid == studentId);
        }
        
        public List<ExerciseGrade> Get(int exerciseId)
        {
            return _context.ExerciseGrades.Where(x => x.Exercise.Id == exerciseId).ToList();
        }
        
    }
}