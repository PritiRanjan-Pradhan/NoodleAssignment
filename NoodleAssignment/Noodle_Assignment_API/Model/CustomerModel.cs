namespace Noodle_Assignment_API.Model
{
    public class CustomerModel
    {
        public string Id { get; set; }
        
    }

    public class CreateCustomer
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Country { get; set; }
    }
    public class MeClientModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

}
