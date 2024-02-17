namespace BasketballManagerAPI.Dto {
    public class BaseEntityResponseDto {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        public Guid CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }
    }
}
