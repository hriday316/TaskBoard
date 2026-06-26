using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Services;

namespace TaskBoard.Infrastructure.Services;

public class FileService(IWebHostEnvironment environment) : IFileService
{
    private readonly IWebHostEnvironment _environment = environment;
    private string[] _allowedTypes = [".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx", ".txt"];
    private const long _maxFileSize = 50 * 1024 * 1024;
    public Task DeleteFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public async Task<List<string>?> UploadFilesAsync(IEnumerable<IFormFile> files)
    {
        var result = new List<string>();
        if (files == null || !files.Any())
        {
            return null;
        }

        foreach (var file in files)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedTypes.Contains(extension))
            {
                throw new InvalidOperationException("File type is not allowed.");
            }

            if (file.Length > _maxFileSize)
            {
                throw new InvalidOperationException("File size exceeds the maximum limit.");
            }

            var uploadsFolder = Path.Combine(_environment.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
            var OriginalFileName = Path.GetFileNameWithoutExtension(file.FileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var uniqueCode = Guid.NewGuid().ToString("N")[..4];
            var fileName = $"{OriginalFileName}_{timestamp}_{uniqueCode}_{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            
            await file.CopyToAsync(stream);
            var fileUrl = $"/uploads/{fileName}";
            if (!string.IsNullOrEmpty(fileUrl))
            {
                result.Add(fileUrl);
            }
        }

        return result;

    }
}
