namespace BasketballManagerAPI.Exceptions {
    public class AuthException: Exception
    {
        public int StatusCode { get; set; }
        public AuthException(string message, int statusCode) : base(message) {
            StatusCode = statusCode;
        }
    }
}
