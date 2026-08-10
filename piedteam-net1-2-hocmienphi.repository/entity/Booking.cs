using piedteam_net1_2_hocmienphi.repository.abstraction;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class Booking : BaseEntity<Guid>
{
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
    // Tìm hiểu về xử lý chênh lệch múi giờ 
    public User User { get; set; }
    public Guid UserId { get; set; }
    
    public Guid MentorId { get; set; }
    public Mentor Mentor { get; set; }
    
    
}