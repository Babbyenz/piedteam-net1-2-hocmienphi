using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.entity;
using piedteam_net1_2_hocmienphi.service.CategoryService;

namespace piedteam_net1_2_hocmienphi.api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController :  ControllerBase 
{
    private readonly AppDbContext _dbContext;
    
    public CategoryController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    // Dependency Injection: Core, không biết = rớt phỏng vấn
    // 1 buổi bàn luận về cái này 
    
    //Dependency Injection: nó là 1 design pattern, giúp giải quyết vấn đề phụ thuộc giữa các module 
    
    // khai báo cho anh các API sau
    // GET All Category 
    // Có Query Param, phần phân trang và cho phép search 
    // Get Category By Id 
    // Create Category
    // Yêu cầu có Body là name và ParentID
    // Update Category 
    // Delete Category 

    [HttpGet("")]
    public IActionResult GetAllCategory()
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.ParentId == null);
        query = query.OrderBy(x => x.Name);
        

        var selectedQuery = query.Select(x => new Response.GetAllParentCategoryResponse()
        {
            Id = x.Id,
            Name = x.Name,
            
        }).ToList();


        var result = selectedQuery.ToList();
        return Ok(result);
        
        // Mục đích của tạo GetAllParentCategoryResponse
        // API này chỉ cần 2 field Id và Name thôi, nhưng mà trong database lúc nào 
        // cũng quăng ra full (id, name, parentId) nhưng api chỉ cần 2 field
        // quăng ra mà k xài thì phí lắm !
    }

    [HttpGet("{id}")]
    public IActionResult GetCategoryById(Guid id)
    {
        
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);

        query = query.Where(x => x.ParentId != null && x.ParentId == id);

        query = query.OrderBy(x => x.Name);
        
        var selectedQuery = query.Select(x => new Response.GetAllParentCategoryResponse()
        {
            Id = x.Id,
            Name = x.Name,
            
        }).ToList();
        
        
        // Mục đích của tạo GetAllParentCategoryResponse
        // API này chỉ cần 2 field Id và Name thôi, nhưng mà trong database lúc nào 
        // cũng quăng ra full (id, name, parentId) nhưng api chỉ cần 2 field
        // quăng ra mà k xài thì phí lắm !
        
         var result = selectedQuery.ToList();
        return Ok(result);
        // Lấy tất cả User trong database
        return Ok("Đây là tất cả User trong databases");
    }

    [HttpPost("")]
    public IActionResult CreateCategory(Request.CreateCategoryRequest requestBody)
        //Request.CreateUserRequest request
    {
        // requestBody nó body do FE truyền cho mình khi FE gọi API 
        // có nghĩa rằng là nó muốn tạo 1 Record - 1 Dòng dưới Database của mình 
        // 1 Dòng dưới database sẽ có các field: Id, Name, ParentId
        
        // Lúc này Backend phải tạo ra 1 object từ class Category và insert và DB
        // Bởi vì Category Table dưới DB nó được tạo từ class Category 
        // thế nên để mà add dữ liệu vào Table Category thì mình phải 
        // 1 object 


        var newCategory = new Category()
        {
            Id = Guid.NewGuid(),
            Name = requestBody.Name,
            ParentId = requestBody.ParentID
        };
        
        _dbContext.Categories.Add(newCategory);
        
        _dbContext.SaveChanges();
        // Lấy tất cả Category trong database
        return Ok("Đã thêm Category mới");
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteCategoryById(Guid id)
    {
        // Có Id rồi, thì bây giờ mình phải móc (lấy) thằng category trong DB ra, sau đó xóa nó 
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == id);
        var category = query.FirstOrDefault();
        if (category != null)
        {
            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }
        
        
        return Ok("Category được xóa là" + id);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCategoryById(Guid id, Request.UpdateCategoryRequest requestBody)
    {
        var query = _dbContext.Categories.Where(x => x.IsDeleted == false);
        query = query.Where(x => x.Id == id);
        var category = query.FirstOrDefault();
        if (category != null)
        {
            category.Name = requestBody.Name;
            category.ParentId = requestBody.ParentID;
            _dbContext.Categories.Update(category);
            _dbContext.SaveChanges();
            
        }
        return Ok("Đây là Category có id là: " +id);
    }
    
    
}