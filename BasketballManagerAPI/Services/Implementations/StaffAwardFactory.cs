using BasketballManagerAPI.Services.Interfaces;

namespace BasketballManagerAPI.Services.Implementations {
    public class StaffAwardServiceFactory : IStaffAwardServiceFactory {
        private readonly IServiceProvider _serviceProvider;

        public StaffAwardServiceFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public IStaffAwardService CreatePlayerAwardService() {
            return _serviceProvider.GetRequiredService<PlayerAwardService>();
        }

        public IStaffAwardService CreateCoachAwardService() {
            return _serviceProvider.GetRequiredService<CoachAwardService>();
        }
    }
}
