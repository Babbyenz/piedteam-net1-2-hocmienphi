namespace piedteam_net1_2_hocmienphi.service.CategoryService;

public class Request
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public Guid? ParentID { get; set; }
        
        
    }

    public class UpdateCategoryRequest : CreateCategoryRequest;
    
}