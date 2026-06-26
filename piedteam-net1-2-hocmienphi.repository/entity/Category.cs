using piedteam_net1_2_hocmienphi.repository.abtraction;

namespace piedteam_net1_2_hocmienphi.repository.entity;

public class Category : BaseEntity<Guid>
{
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }
    
    public ICollection<Category> Children { get; set; }
    
    public ICollection<MentorCategory>  MentorCategories { get; set; }
    
    
    // Nhờ khai báo ParentId và Parent 
    // mà Entity Framwork sẽ tự động hiểu rằng  
    // giữa 2 trường này có mối quan hệ với nhau 
    // và sẽ tự động tạo ra 1 foreign key trong database liên kết giữa 2 bảng Category với nhau 
    
    // Nhờ việc khai báo ICollection<Category> Children
    // mà Entity Framwork sẽ tự động hiểu rằng 
    // và tạo ra 1 mối quan hệ 1 - N 
    
    public ICollection<ApplyRequestCategory> ApplyRequestCategories { get; set; } 
}