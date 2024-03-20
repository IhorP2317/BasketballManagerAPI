namespace BasketballManagerAPI.Exceptions {
    public class ApiException : Exception {
        public int StatusCode { get; }
        public string? Response { get; }
        public IReadOnlyDictionary<string, IEnumerable<string>>? Headers { get; }

        public ApiException(string message, int statusCode, string? response = null, IReadOnlyDictionary<string, IEnumerable<string>>? headers = null, Exception? innerException = null)
            : base(message, innerException) {
            StatusCode = statusCode;
            Response = response;
            Headers = headers;
        }
    }
}
