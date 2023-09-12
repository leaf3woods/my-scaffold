﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyScaffold.Core
{
    public static class Options
    {
        public static JsonSerializerOptions CustomJsonSerializerOptions { get; set; } = new JsonSerializerOptions
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }
}