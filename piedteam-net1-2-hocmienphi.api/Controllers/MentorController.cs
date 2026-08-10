using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.service.Mentor;

namespace piedteam_net1_2_hocmienphi.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MentorController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public MentorController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    [HttpGet("")]
    public IActionResult GetMentors
    (string? searchTerm = null,
        int page = 1, int pageSize = 10,
        DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null,
        List<Guid>? categoryIds = null
    )
    {
        var query = _dbContext.Mentors
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.User.FirstName.Contains(searchTerm) ||
                x.User.LastName.Contains(searchTerm));
        }
        

     
        var selectedQuery = query.Select(x => new Response.MentorResponse()
        {
            Id = x.Id,
            FirstName = x.User.FirstName,
            LastName = x.User.LastName,
            Email =  x.User.Email,
            OrganizeName = x.OrganizationName,
            CurrentPosition = x. CurrentPosition,
            
            Categories = x.MentorCategories
                .Select(mc =>
                new service.CategoryService.Response.GetAllParentCategoryResponse()
                {
                    Id = mc.Category.Id,
                    Name = mc.Category.Name
                }).ToList()
        });

        selectedQuery = selectedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var result = selectedQuery.ToList();

        return Ok(result);
    }
}
