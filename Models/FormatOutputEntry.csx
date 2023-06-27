#!/usr/bin/env dotnet-script

public class FormatOutputEntry
{
    public DocumentId DocumentId { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public List<FileChanges> FileChanges { get; set; }
}

public class DocumentId
{
    public string Id { get; set; }
}

public class FileChanges
{
    public int LineNumber { get; set; }
    public int CharNumber { get; set; }
    public string DiagnosticId { get; set; }
    public string FormatDescription { get; set; }
}