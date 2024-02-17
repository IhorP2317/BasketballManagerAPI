namespace BasketballManagerAPI.Models {
    public class Coach:Staff {
        public CoachStatusId StatusId { get; set; }

        public CoachStatus CoachStatus { get; set; } = null!;
        public Specialty Specialty { get; set; }


    }
}
