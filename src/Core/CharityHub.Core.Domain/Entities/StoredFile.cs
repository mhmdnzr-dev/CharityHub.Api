namespace CharityHub.Core.Domain.Entities;

using Common;

public sealed class StoredFile : BaseEntity
{
    public string FileName { get; private set; }
    public string FilePath { get; private set; }
    public string FileType { get; private set; }


    public StoredFile(string fileName, string filePath, string fileType)
    {
        FileName = fileName;
        FilePath = filePath;
        FileType = fileType;
    }
}