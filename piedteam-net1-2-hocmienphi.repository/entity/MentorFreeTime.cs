using piedteam_net1_2_hocmienphi.repository.abtraction;
using piedteam_net1_2_hocmienphi.repository.enums;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class MentorFreeTime : BaseEntity<Guid>
{
    public MentorFreeTimeDateOfWeek DateOfWeek { get; set; }
    public DateTimeOffset StartTime { get; set; }
    public DateTimeOffset EndTime { get; set; }
   
    public Guid MentorId { get; set; }
    public Mentor Mentor { get; set; }
    
}