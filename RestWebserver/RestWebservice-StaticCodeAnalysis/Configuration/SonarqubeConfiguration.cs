﻿using RestWebservice_StaticCodeAnalysis.Interfaces;

namespace RestWebservice_StaticCodeAnalysis.Configuration
{
    public class SonarqubeConfiguration : ISonarqubeConfiguration
    {
        public string ServerUrl { get; set; }
    }
}
