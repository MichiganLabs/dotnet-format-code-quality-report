#!/usr/bin/env dotnet-script

public enum Severity
{
    Info,
    Minor,
    Major,
    Critical,
    Blocker,
}

public static class SeverityMapping
{
    private const string info = "Info";
    private const string minor = "Minor";
    private const string major = "Major";
    private const string critical = "Critical";
    private const string blocker = "Blocker";

    public static Severity MapDotnetFormatSeverityToEnum(string severity)
    {
        switch (severity.ToLower())
        {
            case "info":
                return Severity.Info;
            case "warning":
                return Severity.Minor;
            case "error":
                return Severity.Major;
            default:
                throw new InvalidOperationException($"{severity} severity level does not map to a Code Quality severity level");
        }
    }

    public static string MapSeverityToCodeQualityString(Severity severity)
    {
        switch (severity)
        {
            case Severity.Info:
                return info;
            case Severity.Minor:
                return minor;
            case Severity.Major:
                return major;
            case Severity.Critical:
                return critical;
            case Severity.Blocker:
                return blocker;
            default:
                throw new InvalidOperationException($"{severity} severity level is not currently mapped to a string");
        }
    }
}