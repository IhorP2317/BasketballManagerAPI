namespace BasketballManagerAPI.Models {
    public class Coach:Staff {
        public CoachStatusId CoachStatusId { get; set; }

        public CoachStatus CoachStatus { get; set; } = null!;
        public Specialty Specialty { get; set; }


    }
}
