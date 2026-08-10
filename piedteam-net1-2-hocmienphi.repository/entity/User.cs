using piedteam_net1_2_hocmienphi.repository.abstraction;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class User: BaseEntity<Guid>
// Khi kế thừa, thì thằng cha (Class hoặc Abtract Class) luôn nằm đầu tiên sau dấu :
// Những thằng nằm sau thằng cha thì là interface 
{
    
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Age { get; set; }
    
    public string? Avatar { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // Admin, Mentor, Student 
    public string? Phone { get; set; }
    
    public string Nickname { get; set; }
    
    public string? ResetPasswordToken { get; set; }
    
    public DateTime? ResetPasswordTokenExpiredAt { get; set; }
    
    public Mentor? Mentor { get; set; }
    
    public ICollection<ApplyRequest> ApplyRequests { get; set; }
    
    public ICollection<Booking> Bookings { get; set; }
}