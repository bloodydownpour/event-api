using EventList.Domain.Data;

namespace EventList.Domain.CommandData
{
    public class ToggleAdminCommandData
    {
        public User User { get; set; }
    }
    public class UpdateUserPfpCommandData
    {
        public User User { get; set; }
        public string FileName { get; set; }
    }

    public class RegisterUserInEventCommandData
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }

    public class RetractUserFromEventCommandData
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }

    public class GetRefreshTokenCommandData
    {
        public string AccessToken { get; set; }
    }

    public class LoginCommandData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AddUserCommandData
    {
        public User User { get; set; }
    }

    public class RefreshTokenCommandData
    {
        public string Token { get; set; }
    }

    public class ClearRefreshTokenCommandData
    {
        public RefreshToken RefreshToken { get; set; }
    }
}