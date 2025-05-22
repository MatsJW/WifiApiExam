namespace WifiAPIExam.Services;

public interface IImportService
{
     /// <summary>
     /// Imports prices from the given URL and stores them in the database.
     /// </summary>
     /// <param name="directoryPath">The URL to import prices from.</param>
     /// <returns>A task representing the asynchronous operation.</returns>
     Task ImportFromDirectoryAsync(string directoryPath);
}