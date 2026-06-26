

namespace piedteam_net1_2_hocmienphi.service.UserService;

public class Response
{
    public class GetUserResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public string Email { get; set; }
    }

    public class LoginResponse 
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role  { get; set; }
        
    }
}