using Microsoft.AspNetCore.Http;

namespace piedteam_net1_2_hocmienphi.service.UserService;

public class Request
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public string Age { get; set; }
        
    }

    public class UpdateUserRequest: CreateUserRequest
    {
        public IFormFile? Avatar { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    
    public class ForgotPasswordRequest
    {
        public string Email { get; set; }
        
    }
    
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    
}

