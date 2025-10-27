using File = System.IO.File;

namespace EShop.Application.Utilities
{
    public static class Logger
    {
        private static readonly string _logFolderPath = Path.Combine(AppContext.BaseDirectory, "Logs");

        private static string GetLogFilePath()
        {
            // Generate a fresh path each time based on the current date
            var fileName = $"log_{DateTime.Now:yyyy-MM-dd}.txt";
            return Path.Combine(_logFolderPath, fileName);
        }

        public static void ShowError(Exception ex)
        {
            try
            {
                if (!Directory.Exists(_logFolderPath))
                    Directory.CreateDirectory(_logFolderPath);

                var filePath = GetLogFilePath();

                var errorMessage = $"[{DateTime.Now}] - {DateTime.Now.ToStringShamsiDate()}\nError ===> {ex.Message}\n{ex}\n***************************************************************************************************";

                File.AppendAllLines(filePath, new List<string> { errorMessage });
            }
            catch
            {
                // Avoid throwing inside logger
            }
        }
    }
}
