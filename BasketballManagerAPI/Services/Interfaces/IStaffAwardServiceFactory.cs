namespace BasketballManagerAPI.Services.Interfaces {
    public interface IStaffAwardServiceFactory {
        IStaffAwardService CreatePlayerAwardService();
        IStaffAwardService CreateCoachAwardService();
    }
}
