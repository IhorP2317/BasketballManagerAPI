using System.Web;

namespace Security.Helpers {
    public class TokenDecoder {
        public static string DecodeTokenFromUrl(string encodedToken) {
            
            string decodedToken = HttpUtility.UrlDecode(encodedToken);

            return decodedToken;
        }
    }
}
