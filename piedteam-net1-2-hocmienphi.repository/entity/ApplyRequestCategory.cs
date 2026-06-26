using piedteam_net1_2_hocmienphi.repository.abtraction;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class ApplyRequestCategory : BaseEntity<Guid>
{
    public Guid ApplyRequestId { get; set; }
    public ApplyRequest ApplyRequest { get; set; }
    
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

}