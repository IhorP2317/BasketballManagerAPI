using BasketballManagerAPI.Helpers.ValidationAttributes;

namespace BasketballManagerAPI.Dto {
    public class BaseEntityRequestDto {
        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset? ModifiedTime { get; set; }
        [NonEmptyGuid(ErrorMessage = "Created by ID must be a non-empty GUID.")]
        public Guid CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }
    }
}
