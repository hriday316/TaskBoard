using Microsoft.AspNetCore.Http;

namespace TaskBoard.Application.Interfaces.Services;

public interface IFileService
{
    Task <List<string>?> UploadFilesAsync(IEnumerable<IFormFile> files);
    Task DeleteFileAsync(string filePath);
}