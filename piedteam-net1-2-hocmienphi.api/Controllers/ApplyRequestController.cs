using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.entity;
using piedteam_net1_2_hocmienphi.repository.enums;
using piedteam_net1_2_hocmienphi.service.CategoryService;
using Request = piedteam_net1_2_hocmienphi.service.ApplyRequestService.Request;
using Response = piedteam_net1_2_hocmienphi.service.ApplyRequestService.Response;

namespace piedteam_net1_2_hocmienphi.api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ApplyRequestController :  ControllerBase 
{
    private readonly AppDbContext _dbContext;
    
    /*
     * // Flow thiết kế API như thế nào 
       // Xác định có những API nào
           // API này sẽ làm nhiệm vụ gì
           // Xác định các endpoint (Method, Url)
           // Ai sẽ gọi API này
           // Ngay lập tức chui vô controller khai báo luôn 
               // Để nó ra Swagger cho nó an tâm luôn
           
       // Để làm hành động này thì cần những yêu cầu gì (Input)
           // Ngay lập tức List ra luôn 
           // Đi vào Entity ngắm ngía xem có field gì 
           // Sau khi xác định đc input rồi, tức tốc đi vào Service 
               // Chui ngay vào folder Request tạo luôn 
           // Sau đó lắp đồ chơi này vào cái API mà nó cần ở Controller
           // Sau khi mà xác định được input là gì 
           // Quay ra hỏi FE có cần tạo gì không 
           // Nếu cần -> Đi ngay vào Service và tạo class Response
           // Nếu không -> thì thôi 
    
       // Sau khi Controller đủ đồ chơi rồi, có input và output rồi 
           // Implement logic xử lí bên trong 
     */
    
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
        
       /*
        *  // Apply Request Category là bảng lưu trữ những lĩnh vực mà người Mentor Apply 
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
                  * /
                 // Làm sao để biến 1 List<Guid> thành 1 List<ApplyRequestCategory>
                 // Dùng Select để ánh xạ, x lúc này tượng trưng cho 1 Category
                 // FE chỉ truyền cho mình 1 list requestBody.CategoryIds
                 // nhưng mà những dữ liệu mà mentor mong muốn được mentoring category thì nó nằm 
                 // ở bảng ApplyRequestCategory 
        */
        
        
        var applyRequestCategories = requestBody.CategoryIds.Select(x => new ApplyRequestCategory()
            {
                Id = Guid.NewGuid(),
                ApplyRequestId = request.Id,
                CategoryId = x
            }
        );
        
        //req1 + cate1 (kinh tế)
        //req2 + cate2 (Phần mềm)
        
        _dbContext.ApplyRequestCategories.AddRange(applyRequestCategories);
        _dbContext.SaveChanges();
        
        return Ok();
    }
    
    
    [HttpGet("")]
    public IActionResult GetAllApplyRequest
    (string? searchTerm = null,
        ApplyRequestStatus? status = null,
        int page = 1, int pageSize = 10,
        DateTimeOffset? fromDate = null, DateTimeOffset? toDate = null,
        List<Guid>? categoryIds = null
    )
    {
        var query = _dbContext.ApplyRequests
            .Where(x => !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x =>
                x.Description.Contains(searchTerm) ||
                x.User.FirstName.Contains(searchTerm) ||
                x.User.LastName.Contains(searchTerm));
        }
        
        // categoryIds: là những CategoryId mà FE muốn tìm kiếm | muốn filter
        // tôi muốn tìm những lá đơn thuộc loại Category này 
        // Ví dụ: Tôi muốn tìm những lá đơn thuộc loại Category "Kinh tế" 
            //
            //

        if (categoryIds != null && categoryIds.Count > 0)
        {
            query = query.Where(x => x.ApplyRequestCategories.Any(apC => categoryIds.Contains(apC.CategoryId)));
        }
        

        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        var selectedQuery = query.Select(x => new Response.GetApplyRequestResponse()
        {
            Id = x.Id,
            Description = x.Description,
            CvLink = x.CvLink,
            Status = x.Status,
            RejectReason = x.RejectReason,

            User = new service.UserService.Response.GetUserResponse()
            {
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Age = x.User.Age,
                Email = x.User.Email
            },

            Categories = x.ApplyRequestCategories.Select(apC => new service.CategoryService.Response.GetAllParentCategoryResponse()
            {
                Id = apC.Category.Id,
                Name = apC.Category.Name
            }).ToList()
        });
        
        selectedQuery = selectedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        
        var result = selectedQuery.ToList();
        
        return Ok(result);
    }
    
    [HttpGet("me")]
    public IActionResult GetMyApplyRequest(Guid userId,
        string? searchTerm = null,
        ApplyRequestStatus? status = null,
        int page = 1, int pageSize = 10)
    {
        var query = _dbContext.ApplyRequests
            .Where(x => !x.IsDeleted);
        //
        //
        //
        //

        query = query.Where(x => x.UserId == userId);

        if (status != null)
        {
            query = query.Where(x => x.Status == status);
        }

        var selectedQuery = query.Select(x => new Response.GetApplyRequestResponse()
        {
            Id = x.Id,
            Description = x.Description,
            CvLink = x.CvLink,
            Status = x.Status,
            RejectReason = x.RejectReason,

            User = new service.UserService.Response.GetUserResponse()
            {
                FirstName = x.User.FirstName,
                LastName = x.User.LastName,
                Age = x.User.Age,
                Email = x.User.Email
            },

            Categories = x.ApplyRequestCategories.Select(apC => new service.CategoryService.Response.GetAllParentCategoryResponse()
            {
                Id = apC.Category.Id,
                Name = apC.Category.Name
            }).ToList()
        });
        
        selectedQuery = selectedQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize);
        
        var result = selectedQuery.ToList();
        
        return Ok(result);
    }
    
    [HttpGet("{id}")]
    public IActionResult GetApplyRquestDetail(Guid id)
    {
        
        var result = _dbContext.ApplyRequests
            .Where(x => !x.IsDeleted && x.Id == id)
            .Select(x => new Response.GetApplyRequestResponse()
            {
                Id = x.Id,
                Description = x.Description,
                CvLink = x.CvLink,
                Status = x.Status,
                RejectReason = x.RejectReason,

                User = new service.UserService.Response.GetUserResponse()
                {
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName,
                    Age = x.User.Age,
                    Email = x.User.Email
                },

                Categories = x.ApplyRequestCategories.Select(apC => new service.CategoryService.Response.GetAllParentCategoryResponse()
                {
                    Id = apC.Category.Id,
                    Name = apC.Category.Name
                }).ToList()
            })
            .FirstOrDefault();

        return Ok(result);
    }
    
    
    
    [HttpPost("{id}/review")]
    public IActionResult ReviewApplyRequest(Guid id, Request.ReviewApplyRequestRequest requestBody)
    {

        var query = _dbContext.ApplyRequests
            .Where(x => !x.IsDeleted);
        
        query = query.Where(x => x.Id == id);

        query = query.Include(x => x.User)
                        .Include(x => x.ApplyRequestCategories);
        
        var applyRequest = query.FirstOrDefault();

        if (applyRequest == null)
        {
            return NotFound();
        }

        if (requestBody.IsApproved)
        {
            applyRequest.Status = ApplyRequestStatus.Approved;
            applyRequest.User.Role = "Mentor";
            // Nãy anh lừa em ở trong Select x.User 1 cái là ngon luôn, tại sao ở đây API call 1 cái lại bị null 
            // Auto join nó chỉ hoạt động khi mình sử dụng Select thôi 
            // Còn ở đây nếu mà muốn chấm ra User xài ngon ơ á thì mình phải sử dụng 
                // Include để join thủ công 
            
                
            //BTVN: viết tiếp đoạn này, tạo mới entity Mentor và Category cho Mentor đó 

            var mentor = new Mentor()
            {
                Id = Guid.NewGuid(),
                UserId = applyRequest.UserId,
            };
            _dbContext.Mentors.Add(mentor);
            _dbContext.SaveChanges();
            
            var mentorCategories = applyRequest.ApplyRequestCategories
                .Select(x => new MentorCategory()
                {
                    Id = Guid.NewGuid(),
                    MentorId = mentor.Id,
                    CategoryId = x.CategoryId
                })
                .ToList();
            
            _dbContext.MentorCategories.AddRange(mentorCategories);
            _dbContext.SaveChanges();

        }
        
        else
        {
            applyRequest.Status = ApplyRequestStatus.Rejected;
            applyRequest.RejectReason = requestBody.Reason;
        }
        
        _dbContext.ApplyRequests.Update(applyRequest);
        _dbContext.SaveChanges();
        
        return Ok();
    }
    
}

//Lấy ra tất cả mentor và phân loại của họ 












