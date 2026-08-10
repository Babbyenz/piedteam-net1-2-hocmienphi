using piedteam_net1_2_hocmienphi.repository.abstraction;
using piedteam_net1_2_hocmienphi.repository.enums;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class ApplyRequest: BaseEntity<Guid>
{
    public string Description { get; set; }
    
    public string CvLink { get; set; }
    
    // Họ sẽ đưa cho mình file CV, sau đó mình sẽ upload lên Cloud 

    public ApplyRequestStatus Status { get; set; } = ApplyRequestStatus.Pending; 
    //Pending, Approved, Rejected

    public string? RejectReason { get; set; } // Lý do bị từ chối, nếu có 

    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public ICollection<ApplyRequestCategory> ApplyRequestCategories { get; set; }
    
}