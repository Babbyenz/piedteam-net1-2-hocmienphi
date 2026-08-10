using piedteam_net1_2_hocmienphi.repository.abstraction;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class Mentor : BaseEntity<Guid>
{
    public string? OrganizationName { get; set; } //FPT Software
    public string? CurrentPosition { get; set; } // Senior Software Engineer
    
    // Tại sao ở đây nên là nullable mà k phải là require:
    // Tại vì khi user trở thành Mentor sau khi họ được Approved
    // Thì lúc này ta tạo mới Mentor 
    // Nếu k đổi thành nullable thì 2 field đó ở đâu ra bây giờ 
    // Flow đúng phải là khi mà họ login lần đầu tiên sau khi được Approve
    // Chúng ta sẽ yêu cầu họ bắt buộc phải điền thông tin bổ sung 
    
    
    
    public User User { get; set; }
    public Guid UserId { get; set; }
    
    public ICollection<MentorCategory> MentorCategories { get; set; }
    
    public ICollection<Booking> Bookings { get; set; }
    public ICollection<MentorFreeTime> MentorFreeTimes { get; set; }
}