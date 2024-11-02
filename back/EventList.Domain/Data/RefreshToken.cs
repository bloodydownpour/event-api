namespace EventList.Domain.Data
{
    public class RefreshToken(string accessToken, Guid UserId, DateTime Expiration)
    {
        public string Token { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        public string AccessToken { get; set; } = accessToken;
        public Guid UserId { get; set; } = UserId;
        public DateTime Expiration { get; set; } = Expiration;
    }
}
