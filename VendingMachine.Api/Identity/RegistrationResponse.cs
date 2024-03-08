namespace VendingMachine.Api.Identity
{
    public class RegistrationResponse
    {
        public bool IsSuccessfulRegistration { get; set; }
        public Guid ActorId { get; set; }
        public IEnumerable<string> Errors { get; set; }

    }
}
