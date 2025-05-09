namespace Web.Models
{
    public class RegisterResponse
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }

        public string Email { get; set; }

        public string ConfirmEmailToken { get; set; }
    }
}
