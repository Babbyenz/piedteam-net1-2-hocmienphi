using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.entity;
using piedteam_net1_2_hocmienphi.service.Models;
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
    private readonly IService _userService;
    
    /*
     * DI - Dependency Injection
     * Nó là 1 từ khóa - Lý thuyết rất quan trọng, xương sống của .NET.
     * Các thư viện đều sử dụng DI, K biết DI thì k code được.
     * Lý thuyết sau xa thì rât là nhiều, nhưng sử dụng thì chỉ có vài dòng.
     * VÍ dụ:
     * + private readonly AppDbContext _dbContext; (Khai báo sử dụng)
     * + public UserController(AppDbContext dbContext, IConfiguration configuration) (Tiêm vào)
       {
           _dbContext = dbContext;
           configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
       }
     *
     * Giải thích DI bằng từ từ
     * 
     * 
     */
    private readonly JwtOptions _jwtOptions = new();

    public UserController(AppDbContext dbContext, IConfiguration configuration,  IService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
        
    }

    [HttpGet("")] // Query Param: pageIndex, pageSize, sẽ thay đổi khi FE truyền vào 
    public async Task<IActionResult>
        GetAllUsers(string searchTerm, int pageIndex = 1,
            int pageSize = 10) // ?: 1 là null, 2 là k truyền mặc định là 0 
    {
         /*
                   từ đó giờ, trong lập trình ở dự án hiện tại, mình chưa có đề cập gì đến
                   việc lập trình bat đồng bộ - điều này có nghĩa lí gì?
                   khi mà N requests gọi đến cùng 1 lúc thì nó xử lí như thế nào (chưa bàn tới)
                   đó giờ mình chỉ lập trình đồng bộ thôi (Synchronous) - có nghĩa
                   khi mà các request tới thì nó xu lí tuần tự
                   tự đặt ra các câu hoi như sau
                   - vậy thì lâp trình ất đồng bộ nghĩa là như thế nào
                   - lập trình bat đồng bộ có phaải thực hiện N request cùng 1 lúc kh
                   - thực hiện N request cùng 1 lúc, hình như thấy hoi giôống song song - parallel
                   - vậy thì bất đồng bộ khác parallel như the nao
                   
                   giải thích đồng bộ và bt đồng bộ khác nhau như thế nào bằng ví dụ pha cà phê
                   - đồng bộ: nhân vien nhận Order -> pha cf -> đưa khách -> rồi sau đó mới nhận khách tiếp theo
                   - điểm đặc biet: trong lúc pha cf có hành động là đợi máy xay cf xong. thì lúc này nếu có khách tơới nhận order
                   thawngf nhân viên bat dong bo sẽ nói rằng: m order gì, km may!!! t kh can biet, t phải đợi máy pha cf xay xong đã
                   rồi t moi tính tip
                   
                   - bất dong bo: linh hoạt hơn, những hành động nào cần phải chờ đợi như là (đợi máy xay cf xay xong) thi chủ đông
                   bỏ qua và nhận 1 hành động mới như là nhận Order mới. sau đó máy pha cf xay xong thì mình nhận kết quả
                   và pha ly nước cho khách cũ
                   
                   thông thường mình cứ nghĩ, bất đồng bộ là người nhân viên xử li 2 order cùng 1 lúc, nhưng kh phải, 
                   nó là 1 dạng làm việc thông minh
                   
                   giải thích bat dong bo và song song và ví dụ quản lí nhà hàng
                   nhà hàng piedteam chi nhánh C#. giar sử nhaf hang của anh có 2 nhan vien la (Binh va Nam). nha hàng phuc vụ 2 mảng 
                   la đồ ăn chay và đồ ăn mặn (cả 2 nhân viên deu lam dc hết)
                   
                   đối voi lai Parallel
                   -> phân công: Bình chỉ dc làm đồ chay, Nam chỉ làm do mặn
                   -> trường hợp ngon nhất: nhà hàng nhan dc các đơn hàng có khoi lượng cong viec của chay va man bang nhau
                       nhan vien phuc vu hết công suất.
                   -> trường hợp tệ nhất: ben đồ chay nổ 100 đơn, bên đồ mặn nổ 0 đơn. lúc này bên Bình thì lam viec xấp mặt,
                   bên Nam thì chill, Bình bảo Nam qua phụ làm, Nam kh phu luon. vi sep Tan đã chia từ đầu r
                   
                   đối với lại bat dong bo: 
                   -> cac ae làm việc hòa thuận voi nhau. ben chay nổ đơn nhieu hon, cả 2 ae góp công vao phụ
                   -> nếu khi nhà hàng quá tải, luc nay chỉ can tuyển thêm nhan vien ma thoi
                   
                   implement vao code
                   tương tu voi JS thi .NET cũng co lap trinh bat dong bo. ben JS mình có Promise thì 
                   bên .NET cũng có Task. Promise = Task
           .NET cung co Async va Await
                   -> Async thi danh gia method nay la 1 hanh dong bat dong bo
                   -> Await: hay để phuong thức nay dc thực thi cho den khi hoan tat, luc nay minh tranh thu di lam cai khac
                   khi ma minh await trong 1 cai ham thi minh phai khai bao cai ham do la async
                   khi ma minh đánh dau 1 ham la async thi có nghĩa rang la cai ham nay sẽ hứa trả cho minh 1 kq
                   hứa (promise = task). luc nay 1 cai ham async phai bat buoc tra ra task
                   quy tac dinh nghia task nhu the nao:
                   -> 1 cai ham thi dau ra (response) thong thuong co 2 gia tri:
                       - void                              -> Task 
                       - 1 list gi do ..., 1 kiểu gi do    -> Task<List<Student>> | Task<int> | Task<string>
                       
                   bat dong bo trong .NET thi có 2 cái ham dac biet nua la WhenAll và WhenAny
                   thi WhenAll de lam cgi
                   vi du: trong 1 cai logic no co 3 cai func deu la bat dong bo het
                       - func 1 thi 3s
                       - func 2 thi 2s
                       - func 3 thi 4s
                   doi voi logic binh thuong
                   await func1
                   await func2
                   await func3
                   -> tong thoi gian ham nay xu li se la 9s
                   doi voi lai khi minh sai WhenAll
                   -> no se lay 3 ket qua cung 1 luc dua. tren func tra ra ket qua lau nhat (func4)
                   -> tong thoi gian se la 4s
                   -> nhung ma neu co 1 task bi loi thi tat ca se dung lai luon
                   var result = await Task.WhenAll(func1, func2, func3);
                   
                   doi voi lai khi minh sai WhenAny
                   -> no se lay 1 ket qua tra ra ket qua nhanh nhat (func2)
                   -> tong thoi gian se la 2s
                   -> nhung ma neu co 1 task bi loi thi ham van se tiep tuc chay, dam bao thang nao tra ra nhanh nhat va kh bi loi
                   var result = await Task.WhenAny(func1, func2, func3);
                   
                   vay thi cau hoi dat ra la. vay minh sai WhenAny di. tai no la nhanh nhat ma
                   
                   sai cach bth, khi nhung ketqua cua cac ham phu thuoc len nhau
                       + vi du luong tao tai khoan
                           -> truy van xuong db xem user co ton tai kh
                           -> tao account va luu xuong db
                           -> neu tao account thanh cong thi gui mail chuc mung
                            
                   sai whenAll khi ketqua cua tung ham kh phu thuoc len nhau
                       + vi du luong la tao mentor, FE dua cho minh 2 thu la UserId va CategoryId
                           -> minh phai verify UserId va CategoryId co ton tai hay kh, neu kh ton tai thi bi loi ForeignKey
                           -> thi o truong hop nay, 2 hanh dong kh phu thuoc lan nhau, nen sai whenAll la toi uu nhat
                           
                   sai whenAny khi minh muon kiem tra xem Service nao la nhanh nhat
                       + thong thuong se dc sai trong Load Balancer, trong 1 he thong lon se co nhieu services.
                       + 1 request se duoc call toi 3 services cung 1 luc, luon luon dam bao co se luon co 1 thang tra ra ketqua
            
         */
         
         var result = await _userService.GetAllUser(searchTerm, pageIndex, pageSize);
       
        // Lấy tất cả User trong database
        return Ok(result);
    }
    

    [HttpGet("{id}")] // Route Param: Id sẽ thay đổi khi FE truyền vào 
    public async Task<IActionResult> GetUserById(Guid id)
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
        
        var result = await selectedQuery.FirstOrDefaultAsync();
        return Ok(result);
    }
    
    [HttpPost("")] // Body: Dữ liệu sẽ được truyền qua body, nên không cần phải đặt tham số trong URL 
    public async Task<IActionResult> CreateUsers(Request.UpdateUserRequest requestBody)
    {
        var result = await _userService.CreateUser(requestBody);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserById(string searchTerm, int pageIndex,
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
        await _dbContext.SaveChangesAsync();
        return Ok("Delete user");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserById(Guid id, Request.UpdateUserRequest requestBody)
    {
        var result = await _userService.UpdateUserById(id, requestBody);
        
        return Ok(result);
    }
    
   [HttpPost("Login")]
public async Task<IActionResult> Login(string email, string password)
{
    //throw new Exception("hihi");
    /*
       // Theo ae tại sao phải login 
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
       
     
   
    // Đầu tiên tìm kiếm cái tài khoản với email đó, có tồn tại hay không 
      // Nếu mà có thì mới tính tiếp được 
           //Tiếp tục so sánh với Password người dùng nhập vào với password có trong database
               // Nếu mà trùng, bạn chính là chủ nhân của tài khoản, tôi sẽ trả ra JWT Token
                   // cho bạn để xác thực và phân quyền 
               // Nếu mà k trùng, mày k phải chủ nhân của tài khoản, cút
       // Nếu mà k tồn tại email thì cút 
    
   */
    
    var result = await _userService.Login(email, password);
    
    // Phần này chỉ là phần cách mà server tạo ra 1 cái Token
    // Nhưng mà,... chưa đủ 
    
    // Chúng ta còn phải cấu hình cách mà server xử lí 1 cái Token 
    // Liệu rằng Token này có thực sự là của mình hay không 
    return Ok(ResponseBuilder.SuccessResponse(result, "Login Successfully", HttpContext.TraceIdentifier));
    
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

