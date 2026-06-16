using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.entity;
using piedteam_net1_2_hocmienphi.repository.enums;
using piedteam_net1_2_hocmienphi.service.CategoryService;
using Request = piedteam_net1_2_hocmienphi.service.ApplyRequestService.Request;

namespace piedteam_net1_2_hocmienphi.api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ApplyRequestController :  ControllerBase 
{
    private readonly AppDbContext _dbContext;
    
    // Flow thiết kế API như thế nào 
        // Xác định có những API nào
            // API này sẽ làm nhiệm vụ gì
            // Xác định các endpoint (Method, Url)
            // Ai sẽ gọi API này
            // Ngay lập tức chui vô controller khai báo luôn 
                // Để nó ra Swagger cho nó an tâm luôn
            
        // Để làm hành động này thì cần những yêu cầu gì (Input)
            // Ngay lập tức List ra luôn 
            // Đi vào Entity ngắm ngiá xem có field gì 
            // Sau khi xác định đc input rồi, tức tốc đi vào Service 
                // Chui ngay vào folder Request tạo luôn 
            // Sau đó lắp đồ chơi này vào cái API mà nó cần ở Controller
            // Sau khi mà xác định được input là gì 
            // Quay ra hỏi FE có cần tạo gì không 
            // Nếu cần -> Đi ngay vào Service và tạo class Response
            // Nếu không -> thì thôi 
    
        // Sau khi Controller đủ đồ chơi rồi, có input và output rồi 
            // Implement logic xử lí bên trong 
    
    public ApplyRequestController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    [HttpPost("")]
    public IActionResult CreateApplyRequest( Request.CreateApplyRequestRequest requestBody)


    {
        var request = new ApplyRequest()
        {
            Id = Guid.NewGuid(),
            UserId = requestBody.UserId,
            Description = requestBody.Description,
            CvLink = requestBody.CvLink,
            Status = ApplyRequestStatus.Pending
        };
        
        _dbContext.ApplyRequests.Add(request);
        _dbContext.SaveChanges();
        
        // Apply Request Category là bảng lưu trữ những lĩnh vực mà người Mentor Apply 
        // FE: public List<Guid> CategoryIds { get; set; }
        // Trong Database thì lưu trữ bằng Entity ApplyRequestCategory 
        /*
         * public class ApplyRequestCategory : BaseEntity<Guid>
           {
               public Guid ApplyRequestId { get; set; }
               public ApplyRequest ApplyRequest { get; set; }
               
               public Guid CategoryId { get; set; }
               public Category Category { get; set; }
           
           }
         */
        // Làm sao để biến 1 List<Guid> thành 1 List<ApplyRequestCategory>
        // Dùng Select để ánh xạ, x lúc này tượng trưng cho 1 Category
        var applyRequestCategories = requestBody.CategoryIds.Select(x => new ApplyRequestCategory()
            {
                Id = Guid.NewGuid(),
                ApplyRequestId = request.Id,
                CategoryId = x
            }
        );
        _dbContext.ApplyRequestCategories.AddRange(applyRequestCategories);
        _dbContext.SaveChanges();
        
        return Ok();
    }
    
    
    [HttpGet("")]
    public IActionResult GetAllApplyRequest()
    {
        
       
        return Ok();
    }
    
    [HttpGet("me")]
    public IActionResult GetMyApplyRequest()
    {
       
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetApplyRquestDetail()
    {
    
        return Ok();
        
    }
    
    
    [HttpPost("{id}/review")]
    public IActionResult ReviewApplyRequest(Guid id)
     
    {
        return Ok();
    }
    
    
}