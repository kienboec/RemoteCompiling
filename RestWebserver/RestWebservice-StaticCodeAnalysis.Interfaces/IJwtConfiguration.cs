﻿namespace RestWebservice_StaticCodeAnalysis.Configuration
{
    public interface IJwtConfiguration
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpiresInSeconds { get; set; }
    }
}