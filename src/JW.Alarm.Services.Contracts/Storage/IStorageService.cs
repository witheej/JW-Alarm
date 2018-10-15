using System.Threading.Tasks;

namespace JW.Alarm.Services.Contracts
{
    public interface IStorageService
    {
        string StorageRoot { get; }
        Task<bool> DirectoryExists(string path);
        Task<bool> FileExists(string path);

        Task<string> ReadFile(string path);
        Task CopyResourceFile(string resourceFilePath, string destinationDirectoryPath, string destinationFileName);
        Task SaveFile(string directoryPath, string name, string contents);
        Task SaveFile(string directoryPath, string name, byte[] contents);
        Task DeleteFile(string path);
    }
}
