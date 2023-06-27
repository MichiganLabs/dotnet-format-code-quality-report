#!/usr/bin/env dotnet-script

#r "nuget: Newtonsoft.Json, 13.0.3"

using Newtonsoft.Json;

public class CodeQualityEntry
{
    [JsonProperty("check_name")]
    public string CheckName { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("engine_name")]
    public string EngineName { get; set; }

    [JsonProperty("fingerprint")]
    public string Fingerprint { get; set; }

    [JsonProperty("location")]
    public CodeQualityLocation Location { get; set; }

    [JsonProperty("severity")]
    public string Severity { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }
}

public class CodeQualityLocation
{
    [JsonProperty("path")]
    public string Path { get; set; }

    [JsonProperty("lines")]
    public CodeQualityLines Lines { get; set; }
}

public class CodeQualityLines
{
    [JsonProperty("begin")]
    public int Begin { get; set; }

    [JsonProperty("end")]
    public int End { get; set; }
}