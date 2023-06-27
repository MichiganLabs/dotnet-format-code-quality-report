#!/usr/bin/env dotnet-script

/*
* Generates a CodeQuality JSON report for GitLab from a given dotnet format report JSON
* The CodeQuality JSON adheres to the spec given here: https://docs.gitlab.com/ee/ci/testing/code_quality.html#implement-a-custom-tool
*/

#r "sdk:Microsoft.NET.Sdk"
#r "nuget: Newtonsoft.Json, 13.0.3"

#load "Enums/Severity.csx"
#load "Helpers/Helpers.csx"
#load "Models/CodeQualityEntry.csx"
#load "Models/FormatOutputEntry.csx"

using Newtonsoft.Json;

var (inputFile, outputFile, workingDirectory) = ParseArgs((List<string>)Args);
var codeQualityEntries = new List<CodeQualityEntry>();

Console.WriteLine($"Generating {outputFile} from {inputFile}.");

// Deserialize JSON directly from the dotnet format report file and convert it into CodeQualityEntry objects
using (StreamReader reader = new StreamReader(inputFile))
{
    var json = reader.ReadToEnd();

    List<FormatOutputEntry> entries = JsonConvert.DeserializeObject<List<FormatOutputEntry>>(json);

    entries.ForEach(entry =>
    {
        entry.FileChanges.ForEach(fileChanges =>
        {
            codeQualityEntries.Add(ConvertFormatEntryToCodeQualityEntry(entry, fileChanges, workingDirectory));
        });
    });
}

// Serialize CodeQuality entries back out to a new JSON
using (StreamWriter file = File.CreateText(outputFile))
{
    JsonSerializer serializer = new JsonSerializer();
    serializer.Serialize(file, codeQualityEntries);
}

Console.WriteLine($"Finished generating {outputFile}!")