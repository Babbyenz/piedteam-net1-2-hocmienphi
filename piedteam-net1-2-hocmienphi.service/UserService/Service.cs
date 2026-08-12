using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using piedteam_net1_2_hocmienphi.repository;
using piedteam_net1_2_hocmienphi.repository.entity;
using piedteam_net1_2_hocmienphi.service.Utils.JwtService;
using MailService = piedteam_net1_2_hocmienphi.service.Utils.Mail;
using MediaService = piedteam_net1_2_hocmienphi.service.Utils.MediaService;
namespace piedteam_net1_2_hocmienphi.service.UserService;


public class Service : IService
{
    private readonly AppDbContext _dbContext;
    private readonly MailService.IService _mailService;
    private readonly MediaService.IService _mediaService;
    private readonly JwtOptions _jwtOptions = new();

    public Service(AppDbContext dbContext, MailService.IService mailService, MediaService.IService mediaService, IConfiguration configuration )
    {
        _dbContext = dbContext;
        _mailService = mailService;
        _mediaService = mediaService;
        configuration.GetSection(nameof(JwtOptions)).Bind(_jwtOptions);
        
    }
    
    public async Task<List<Response.GetUserResponse>> GetAllUser(string? searchTerm, int pageIndex,
        int pageSize)
    {
        var query = _dbContext.Users.Where(x => x.IsDeleted == false);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var sanitizerText = searchTerm.Trim().ToLower();

            query = query
                .Where(x =>
                    x.FirstName.Contains(sanitizerText) ||
                    x.LastName.Contains(sanitizerText) ||
                    x.Email.Contains(sanitizerText)
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
       
        var result = await selectedQuery.ToListAsync();
        // Lấy tất cả User trong database
        return result;
    }

    public async Task<string> CreateUser(Request.CreateUserRequest requestBody)
    {
        var existUser = await _dbContext.Users.AnyAsync(x => x.Email == requestBody.Email);
        if (existUser) throw new Exception("User already exists");
        
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
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

        var mailContent = new MailService.MailContent()
        {
            Body = MailService.MailTemplates.GetHelloMailTemplate( newUser.FirstName + " " + newUser.LastName),
            To = newUser.Email,
            Subject = "Welcome to HocMienPhi System"
        };

        await _mailService.SendMail(mailContent);
        return "Add User thành công";
    }
    
    public async Task<string> UpdateUserById(Guid id, Request.UpdateUserRequest requestBody)
    {
        var query = _dbContext.Users.Where(x => x.IsDeleted == false);

        query = query.Where(x => x.Id == id);
        var user = await query.FirstOrDefaultAsync();

        if (user != null) throw new KeyNotFoundException("User not found");
        {
            user.FirstName = requestBody.FirstName;
            user.LastName = requestBody.LastName;
            user.Email = requestBody.Email;
            user. Age =  requestBody.Age;
            user.Password = requestBody.Password;
            user.Phone = requestBody.Phone;
            if (requestBody.Avatar != null)
            {
                var avatarUrl = await _mediaService.UploadImageAsync(requestBody.Avatar);
                user.Avatar = avatarUrl;
            }
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();

        }
        
        return "Đây là User có id là " +id;
    }
    
    public async Task <string > Login(string email, string password)
{
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
    
    var query = _dbContext.Users.Where(x => x.IsDeleted == false);
    
    query = query.Where(x => x.Email == email);
    
    var user = await query.FirstOrDefaultAsync();

    if (user == null)
    {
       throw new KeyNotFoundException("User not found");
    }

    if (user.Password != password)
    {
       throw new UnauthorizedAccessException("UnAuthorize !");
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
        new Claim("email", user.Email),
        new Claim("userId", user.Id.ToString()),
        new Claim("role", user.Role)
    };

    var token = JwtService.GenerateJwtToken(claims, _jwtOptions);
    return token;
    
}
}