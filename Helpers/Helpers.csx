#!/usr/bin/env dotnet-script

#load "../Enums/Severity.csx"
#load "../Models/CodeQualityEntry.csx"
#load "../Models/FormatOutputEntry.csx"

// From https://www.techiedelight.com/generate-sha-256-hash-of-string-csharp/
using System;
using System.Security.Cryptography;

private string ComputeSHA256(string s)
{
    StringBuilder sb = new StringBuilder();

    // Initialize a MD5 hash object
    using (MD5 md5 = MD5.Create())
    {
        // Compute the hash of the given string
        byte[] hashValue = md5.ComputeHash(Encoding.UTF8.GetBytes(s));

        // Convert the byte array to string format
        foreach (byte b in hashValue)
        {
            sb.Append($"{b:X2}");
        }
    }

    return sb.ToString();
}

public CodeQualityEntry ConvertFormatEntryToCodeQualityEntry(FormatOutputEntry entry, FileChanges fileChanges, string workingDirectory)
{
    var path = entry.FilePath.Replace(workingDirectory, "");
    var diagnosticId = fileChanges.DiagnosticId;

    var splitDescription = fileChanges.FormatDescription.Split(new char[] { ':' }, count: 2);

    // If there's no severity at all, by default map to a warning
    var severityString = splitDescription.Count() == 2 ? splitDescription[0].Replace(diagnosticId, "").Trim() : "warning";
    var mappedSeverityString = SeverityMapping.MapDotnetFormatSeverityToEnum(severityString);
    var severity = SeverityMapping.MapSeverityToCodeQualityString(mappedSeverityString);

    var description = splitDescription.Last().Trim();

    // fingerprint should be hash of file ID + diagnostic ID + line number + char number
    var fingerprintData = $"{entry.DocumentId.Id}-{fileChanges.DiagnosticId}-{fileChanges.LineNumber}-{fileChanges.CharNumber}";
    var fingerprint = ComputeSHA256(fingerprintData);

    return new CodeQualityEntry()
    {
        CheckName = diagnosticId,
        Description = description,
        EngineName = "dotnet-format",
        Fingerprint = fingerprint,
        Location = new CodeQualityLocation()
        {
            Path = path,
            Lines = new CodeQualityLines()
            {
                Begin = fileChanges.LineNumber,
                End = fileChanges.LineNumber,
            }
        },
        Severity = severity,
        Type = "issue"
    };
}

public (string, string, string) ParseArgs(List<string> args)
{
    // Detect args
    if (Args.Count != 3)
    {
        throw new ArgumentException($"{Args.Count} arguments were passed instead of the necessary 3: inputFile, outputFile, and workingDirectory");
    }

    var argList = (List<string>)Args;

    var inputFile = argList[0];
    var outputFile = argList[1];
    var workingDirectory = argList[2];

    return (inputFile, outputFile, workingDirectory);
}