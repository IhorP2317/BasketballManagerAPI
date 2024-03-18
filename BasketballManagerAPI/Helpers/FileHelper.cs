namespace BasketballManagerAPI.Helpers {
    public class FileHelper {
       private  static readonly string[] ImageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };

        public static bool IsImageFile(string fileName) {
            
            string extension = Path.GetExtension(fileName).ToLower();

            return Array.Exists(ImageExtensions, ext => ext == extension);
        }
    }
}
