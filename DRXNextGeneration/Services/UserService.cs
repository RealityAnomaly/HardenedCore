using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.System;
using Microsoft.Extensions.Logging;

namespace DRXNextGeneration.Services
{
    public class UserService
    {
        public User CurrentUser { get; private set; }
        private bool? _genuine;
        private readonly ILogger _logger;

        public UserService(ILoggerFactory factory)
        {
            _logger = factory.CreateLogger(typeof(UserService));
            _logger.LogInformation("User service logging initialised.");
        }

        private async Task<User> GetCurrentUser()
        {
            if (CurrentUser != null)
                return CurrentUser;

            var users = await User.FindAllAsync();
            var user = users.FirstOrDefault(u => u.AuthenticationStatus == UserAuthenticationStatus.LocallyAuthenticated &&
                                                 u.Type == UserType.LocalUser);
            CurrentUser = user;
            return CurrentUser;
        }

        public async Task<bool> IsMachineGenuineDit()
        {
            // cache the state after the first call - don't care about retention after app restarts
            if (_genuine.HasValue) return _genuine.Value;

            var domain = (await (await GetCurrentUser()).GetPropertyAsync(KnownUserProperties.DomainName)).ToString().Split(@"\")[0].ToLower();
            _genuine = domain == "tower.local";
            return _genuine.Value;
        }
    }
}
