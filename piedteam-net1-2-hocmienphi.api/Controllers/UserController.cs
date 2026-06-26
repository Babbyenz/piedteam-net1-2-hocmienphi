using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.entity;
using piedteam_net1_2_hocmienphi.service.UserService;
using piedteam_net1_2_hocmienphi.service.Utils.JwtService;

namespace piedteam_net1_2_hocmienphi.api.Controllers;

[ApiController]
[Route("api/[controller]")]
//NOTE | Annotation 

public class UserController : ControllerBase
{
    /*
     * // Endpoint gọi tên khác là các APIs
       // VD: POST /api/auth/login //Login 
       // VD: POST /api/auth/register //Đăng kí
       // VD: GET /api/user/{id} //Lấy thông tin của 1 User 
       // VD: PUT /api/user/{id} //Cập nhật thông tin của 1 User 
       // VD: DELETE /api/user/{id} //Xóa 1 user 

       // Những Endpoint này làm thế nào nó xuất hiện -> Khai báo trong Controller 

       // Có mấy phương thức | Method của 1 Request 
       // GET, POST, PUT, PATCH, DELETE
       // GET: lấy dữ liệu 
       // POST: Tạo mới dữ liệu 
       // PUT | PATCH: Cập nhật dữ liệu 
       // DELETE: Xóa dữ liệu 

       //1. Tại sao lại sinh ra các METHOD này 
       //2. 1 METHOD mà mình chơi hết cho tất cả được không. VD: 1 POST mình chơi hết(lấy, tạo, update, xóa) 

       // GET khác POST, PUT, DELETE ở chỗ nào ?
       // GET: Thường sẽ k có request body, dữ liệu sẽ được truyền qua query hoặc route 

       // Theo ae có mấy cách để Frontend truyền dữ liệu sang cho BE 
       // Biết là call API, nhưng mà những cái data mà FE gửi thì nó nằm ở đâu trong Request 

       // Thông thường FE truyền Data qua 3 cách: 
       // Query Param: /api/users?name=tan&age=18
       // là những Param được nằm trên URL và sau dấu ? 
       // Như URL ở trên thì chúng ta có 2 Query Param là name = tan và age = 18 

       // Route Param: /api/user/{id} | /api/users/1234
       // là những Param được nằm trên URL và sau dấu / 
       // Như URL ở trên thì chúng ta có 1 Route Param là id = 1234 

       // Body: Thường sẽ dùng cho POST, PUT, PATCH, DELETE

       // Vì GET thông thường sẽ không có BODY, nên chúng ta hạn chế sử dụng cho các API cần bảo mật 
       // GET để login: GET /api/auth/login?email=tan&password=123
       // Thông thường khi Login chúng ta thường xài Method là POST /api/auth/login, data sẽ được giấu ở trong body 

       // Request là gì ? 
       // 1 yêu cầu xuống server, mong muốn server làm gì đó và trả ra kết quả 
       // Ví dụ: 1 HTTP Request Login, mong muốn server xác thực thông tin đăng nhập, và trả ra kết quả là Token hoặc lỗi 
       // Trong 1 HTTP Request thì sẽ có những thành phần sau: 
       // URL: Địa chỉ của API mà chúng ta muốn gọi 
       // Method: GET, POST, PUT, DELETE
       // Header: Chứa các thông tin về Request như là Content-Type, Authorization, v.v.. 
       // Body: Chứa dữ liệu mà chúng ta muốn gửi lên server (thường dùng cho POST, PUT, PATCH, DELETE)

       // ResFul API: Nó là 1 tiêu chuẩn dùng để thiết kế API,
       // dựa trên các phương thức HTTP và các quy tắc về URL
       // để tạo ra các API dễ hiểu, dễ sử dụng, và dễ bảo trì 

       // GetAll: GET /api/user -> Theo chuẩn thì phải đặt các tham số vô để truy vấn 
       // GetAllUsers: GET /api/user/getall
       // GetAllStaff: GET /api/user/getall-staff
       // GetAllStudent: GET /api/user/getall-student 

       // GetById: GET /api/user/{id}
       // Create: POST /api/user
       // Create User: POST /api/user/create-user
       // Update: PUT /api/user/{id}
       // Delete: DELETE /api/user/{id}
     */


    private readonly AppDbContext _dbContext;
    private readonly JwtOptions _jwtOptions = new();

    public UserController(AppDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
    }

    [HttpGet("")] // Query Param: pageIndex, pageSize, sẽ thay đổi khi FE truyền vào 
    public IActionResult
        GetAllUsers(string searchTerm, int pageIndex = 1,
            int pageSize = 10) // ?: 1 là null, 2 là k truyền mặc định là 0 
    {
        
       var query = _dbContext.Users.Where(x => x.IsDeleted == false);

       if (!string.IsNullOrEmpty(searchTerm))
       {
           var cleanUpSearchTerm = searchTerm.Trim();

           query = query
               .Where(x =>
                   x.FirstName.Contains(cleanUpSearchTerm) ||
                   x.LastName.Contains(cleanUpSearchTerm) ||
                   x.Email.Contains(cleanUpSearchTerm)
               );

           // FirstName: Quan, searchTerm: an => Tan.Contain(an) -> True 
       }
       query = query.OrderBy(x => x.Email);

       var selectedQuery = query.Select(x => new Response.GetUserResponse()
       {
           FirstName = x.FirstName,
           LastName = x.LastName,
           Email = x.Email,
           Age = x.Age,
       });

       selectedQuery = selectedQuery
           .Skip((pageIndex - 1) * pageSize)
           .Take(pageSize);
       
       var result = selectedQuery.ToList();
        // Lấy tất cả User trong database
        return Ok(result);
    }
    

    [HttpGet("{id}")] // Route Param: Id sẽ thay đổi khi FE truyền vào 
    public IActionResult GetUserById(Guid id)
    {
        var query = _dbContext.Users.Where(x => x.IsDeleted == false);

        query = query.Where(x => x.Id.Equals(id));

        var selectedQuery = query.Select(x => new Response.GetUserResponse()
        {
            FirstName = x.FirstName,
            LastName = x.LastName,
            Age = x.Age,
            Email = x.Email,
        });
        
        var result = selectedQuery.FirstOrDefault();
        return Ok(result);
    }
    
    [HttpPost("")] // Body: Dữ liệu sẽ được truyền qua body, nên không cần phải đặt tham số trong URL 
    public IActionResult CreateUsers(Request.UpdateUserRequest requestBody)
    {
        
        var newUser = new User()
        {
            FirstName = requestBody.FirstName,
            LastName = requestBody.LastName,
            Age = "",
            Email =  requestBody.Email,
            Password = requestBody.Password,
            Phone = "",
            Role = "Student"
        };
        
        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        return Ok("Add new user");
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUserById(string searchTerm, int pageIndex,
        int pageSize,Guid id, Request.UpdateUserRequest requestBody)
    {
        var newUser = new User()
        {
            FirstName = requestBody.FirstName,
            LastName = requestBody.LastName,
            Age = "",
            Email =  requestBody.Email,
            Password = requestBody.Password,
            Phone = "",
            Role = "Student"
        };
        
        _dbContext.Users.Add(newUser);
        _dbContext.SaveChanges();
        return Ok("Delete user");
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUserById(Guid id, Request.UpdateUserRequest requestBody)
    {
        var query = _dbContext.Users.Where(x => x.IsDeleted == false);

        query = query.Where(x => x.Id == id);
        var user = query.FirstOrDefault();

        if (user != null)
        {
            user.FirstName = requestBody.FirstName;
            user.LastName = requestBody.LastName;
            user.Email = requestBody.Email;
            user. Age =  requestBody.Age;
            user.Password = requestBody.Password;
            user.Phone = requestBody.Phone;
            
            
        }
        
        return Ok("Update User");
    }
    
   [HttpPost("Login")]
public IActionResult Login(string email, string password)
{
    
    /*
     * // Theo ae tại sao phải login 
       // Giới hạn quyền hạn được gọi đến các resource
       // Ví dụ: Bạn phải là User (Đã đăng kí hệ thống) thì bạn mới đc mua hàng
       
       // Authentication và Authorization 
       
       // Authentication: Bạn có được quyền vào hệ thống của tôi kh 
       // Authorization: Sau khi vào hệ thống của tôi rồi, thì bạn có quyền gì 
           // Admin có quyền tạo 
           // Mentor thì có quyền tạo lịch rãnh 
           
       // Vậy thì thông thường, chúng ta thường dùng kĩ thuật gì để xác thực và phân quyền 
       // Thông thường mình hay sử dụng JWT để xác thực và phân quyền 
       // JWT: JSon Web Token: Là 1 chuỗi Token được mã hóa
           // truyền giữa client(FE) và Server(BE)
           // để xác thực và phân quyền cho người dùng 
       
       // JWT gồm 3 phần: 
       // Header: Chứa thông tin về thuật toán mã hóa và loại token 
       // Payload: Chứa thông tin về người dùng và các quyền hạn của người dùng 
       // Signature: Chứa chữ ký số để xác thực Token(sign(header+payload, secret))
       
     */
   /*
    * // Đầu tiên tìm kiếm cái tài khoản với email đó, có tồn tại hay không 
      // Nếu mà có thì mới tính tiếp được 
           //Tiếp tục so sánh với Password người dùng nhập vào với password có trong database
               // Nếu mà trùng, bạn chính là chủ nhân của tài khoản, tôi sẽ trả ra JWT Token
                   // cho bạn để xác thực và phân quyền 
               // Nếu mà k trùng, mày k phải chủ nhân của tài khoản, cút
       // Nếu mà k tồn tại email thì cút 
    */
   
    var query = _dbContext.Users.Where(x => x.IsDeleted == false);
    
    query = query.Where(x => x.Email == email);
    
    var user = query.FirstOrDefault();

    if (user == null)
    {
        return BadRequest("Email không tồn tại");
    }

    if (user.Password != password)
    {
        return BadRequest("Password không đúng");
    }
    
    // Claim đại diện cho các thông tin nằm trong payload của Jwt
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
        new Claim(ClaimTypes.Role, user.Role),

        // Quan trọng: Claim này(new Claim(ClaimTypes.Role, user.Role),)
        // Sẽ giúp mình phân quyền
        new Claim("userId", user.Id.ToString()),
        new Claim("Role", user.Role)
    };

    var token = JwtService.GenerateJwtToken(claims, _jwtOptions);
    
    
    return Ok();
}

[HttpPost("ForgotPassword")]
public IActionResult ForgotPassword([FromBody] Request.ForgotPasswordRequest requestBody)
{
    if (string.IsNullOrWhiteSpace(requestBody.Email))
    {
        return BadRequest("Email không được để trống");
    }

    var email = requestBody.Email.Trim().ToLower();

    var user = _dbContext.Users
        .FirstOrDefault(x =>
            x.IsDeleted == false &&
            x.Email.ToLower() == email);

    if (user == null)
    {
        return Ok("Nếu email tồn tại trong hệ thống, link đổi mật khẩu sẽ được gửi.");
    }

    var token = Guid.NewGuid().ToString("N");

    user.ResetPasswordToken = token;
    user.ResetPasswordTokenExpiredAt = DateTime.UtcNow.AddMinutes(15);

    _dbContext.SaveChanges();

    return Ok(new
    {
        Message = "Tạo token đổi mật khẩu thành công. Bản demo trả token để test.",
        Token = token,
        ExpiredAt = user.ResetPasswordTokenExpiredAt
    });
}

[HttpPost("ResetPassword")]
public IActionResult ResetPassword([FromBody] Request.ResetPasswordRequest requestBody)
{
    if (string.IsNullOrWhiteSpace(requestBody.Email) ||
        string.IsNullOrWhiteSpace(requestBody.Token) ||
        string.IsNullOrWhiteSpace(requestBody.NewPassword))
    {
        return BadRequest("Email, token và mật khẩu mới không được để trống");
    }

    var email = requestBody.Email.Trim().ToLower();

    var user = _dbContext.Users
        .FirstOrDefault(x =>
            x.IsDeleted == false &&
            x.Email.ToLower() == email &&
            x.ResetPasswordToken == requestBody.Token);

    if (user == null)
    {
        return BadRequest("Token không hợp lệ");
    }

    if (user.ResetPasswordTokenExpiredAt == null ||
        user.ResetPasswordTokenExpiredAt < DateTime.UtcNow)
    {
        return BadRequest("Token đã hết hạn");
    }

    user.Password = requestBody.NewPassword;
    user.ResetPasswordToken = null;
    user.ResetPasswordTokenExpiredAt = null;

    _dbContext.SaveChanges();

    return Ok("Đổi mật khẩu thành công");
}


    
}

//BTVN:
// Tạo mới user
// GetAll User theo phân trang và Search, Orderby các thứ 
// Get Usser By Id 

