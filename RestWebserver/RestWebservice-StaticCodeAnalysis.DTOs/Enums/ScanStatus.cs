﻿using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RestWebservice_StaticCodeAnalysis.DTOs.Enums
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ScanStatus
    {
        /// <summary>
        /// Enum PendingEnum for pending
        /// </summary>
        [EnumMember(Value = "pending")]
        Pending = 0,
        /// <summary>
        /// Enum AvailableEnum for available
        /// </summary>
        [EnumMember(Value = "available")]
        Available = 1,
        /// <summary>
        /// Enum AvailableEnum for failed
        /// </summary>
        [EnumMember(Value = "failed")]
        Failed = 2
    }
}
