namespace piedteam_net1_2_hocmienphi.service.Mentor;

public class Response
{
    public class MentorResponse
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? OrganizeName { get; set; }
        public string? CurrentPosition { get; set; } 
        public List<CategoryService.Response.GetAllParentCategoryResponse> Categories { get; set; }
    }
}