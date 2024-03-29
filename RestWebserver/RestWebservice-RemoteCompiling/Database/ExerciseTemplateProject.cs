﻿using System;
using System.Collections.Generic;

namespace RestWebservice_RemoteCompiling.Database
{
    public class ExerciseTemplateProject
    {
        public int Id
        {
            get;
            set;
        }

        public DateTime LastModified
        {
            get;
            set;
        } = DateTime.Now;

        public string ProjectName
        {
            get;
            set;
        }

        public virtual List<ExerciseTemplateFiles> Files
        {
            get;
            set;
        } = new List<ExerciseTemplateFiles>();

        public ProjectType ProjectType
        {
            get;
            set;
        }
    }
}