namespace piedteam_net1_2_hocmienphi.service.CategoryService;

public class Category
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public Guid ParentID { get; set; }
    }
}