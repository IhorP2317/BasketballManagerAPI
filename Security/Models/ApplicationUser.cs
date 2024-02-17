namespace Security.Models {
    public class ApplicationUser {
        public Guid Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!;

    }
}
