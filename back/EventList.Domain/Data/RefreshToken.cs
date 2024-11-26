namespace EventList.Domain.Data
{
    public class RefreshToken(Guid UserId, DateTime Expiration)
    {
        public string Token { get; set; } = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        public Guid UserId { get; set; } = UserId;
        public DateTime Expiration { get; set; } = Expiration;
    }
}
