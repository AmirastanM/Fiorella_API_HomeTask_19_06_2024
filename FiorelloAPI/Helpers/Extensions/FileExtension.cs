namespace FiorelloAPI.Helpers.Extensions
{
    public static class FileExtension
    {
        public static bool CheckFileType(this IFormFile file, string pattern)
        {
            return file.ContentType.Contains(pattern);
        }

        public static bool CheckFileSize(this IFormFile file, int size)
        {
       
            return file.Length / 1024 < size;
        }

        public static async Task SaveFileToLocalAsync(this IFormFile file, string path)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Fail can't be empty, please add Image");

            string response = Path.GetDirectoryName(path);
            if (!Directory.Exists(response))
            {
                Directory.CreateDirectory(response);
            }

            using FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
        }

        public static void DeleteFileFromLocal(this string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string GenerateFilePath(this IWebHostEnvironment env, string folder, string fileName)
        {
            return Path.Combine(env.WebRootPath, folder, fileName);
        }
    }
}

