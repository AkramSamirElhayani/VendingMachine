namespace VendingMachine.Api.Identity
{
    public class AuthResponse
    {
        public bool IsAuthSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public Guid ActorId { get; set; }
    }
}
