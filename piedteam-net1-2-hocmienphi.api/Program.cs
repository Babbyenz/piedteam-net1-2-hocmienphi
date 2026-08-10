using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.api.Extensions;
using piedteam_net1_2_hocmienphi.api.Middlewares;
using piedteam_net1_2_hocmienphi.repository;
using UserService = piedteam_net1_2_hocmienphi.service.UserService;
using MailService = piedteam_net1_2_hocmienphi.service.Utils.Mail;
using MediaService = piedteam_net1_2_hocmienphi.service.Utils.MediaService;
using CloudinaryService = piedteam_net1_2_hocmienphi.service.Utils.CloudinaryService;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddJwtServices(builder.Configuration);
builder.Services.AddSwaggerServices();

builder.Services.AddScoped<UserService.IService, UserService.Service>();
builder.Services.AddScoped<MailService.IService, MailService.Service>();
builder.Services.AddScoped<MediaService.IService, CloudinaryService.Service>();

builder.Services.AddTransient<GlobalExceptionHandlerMiddleware>();


// Từ dòng app này trở lên trên, khai báo những đồ chơi mà mình xài, không cần quan tâm thứ tự
var app = builder.Build();
// Từ dòng app này trở xuống, apply những đồ chơi vào sever, quan trọng thứ tự apply

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
// Phải đặt ở bên dưới app 
// Để mọi request đều phải đi qua nó, có trường hợp nào quăng lỗi còn xử lí nhanh luôn 


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Phải đặt phía sau Authen Author, vì xác thực phân quyền rồi, tui mới cho vào Controller chứ 

app.Run();

// Kiến trúc 3 Layer (Tầng)

// Tầng API
    // Chịu trách nhiệm khai báo các endpoint, nhận request, trả response
    // Config hệ thống 
    // Tầng API này gọi tới Service

// Tầng Service
    // Chịu trách nhiệm xử lý nghiệp vụ 
    // Tương tác với tầng Repository để lấy dữ liệu
    // Tầng Service này gọi tới Repository

// Tầng Repository
    // Chịu trách nhiêm tương tác với Database
    // Cấu hình những thứ liên quan đến Database
    
// Anh có 1 cái Req là đăng nhập vào hệ thống 
    // Tầng API: Muốn đăng nhập vào hệ thống 
        // Chui vô đây nè: POST /api/auth/login 
            // Nhận request body {email: "quan", password: "123"}
    
    // Tầng API lúc này gọi xuống tầng Service có cái hàm là 
        // Xử lý login: LoginHandler(email, password)
        // Lúc này hàm login trong Service hãy chạy như sau 
            // Kiểm tra email hoặc người dùng có tồn tại trong Database hay không 
            // Người dùng này có bị banned hay k 
            // Nếu có lỗi thì trả về lỗi 
            // Nếu k có lỗi thì trả về token đăng nhập

    // Tầng Service lúc này gọi xuống tầng Repository có cái hàm là 
        // GetUserByEmail(email)
        // Hàm này sẽ chạy câu lệnh SQL để lấy thông tin người dùng ra khỏi database

// Một quá trình phát triển phần mềm thường sẽ trải qua:
    // Đi tìm hiểu nhu cầu khách hàng 
        // Hiểu được nhu cầu rồi -> Phân tích các Requirement 
            // Tìm hiểu thêm trên mạng (cái này phải học của BA)
    // Sau đó, dựa vào các Requirement, chúng ta thiết kế ra hệ thống 
        // Thiết kế ra kiến trúc hệ thống 
        // Thiết kế ra DB (ERD)
    // Implement các Requirement lên Code 
    // Test các Requirement lên Code 
    // Đóng gói - deploy - giao khách hàng 
    // Lụm Tiền 
    // Maintain 

// Nơi cho các học sinh đi vào nền tảng tìm kiếm các Mentor
    // để học tập, trao đổi kiến thức, kinh nghiệm với nhau 
    // Khi mà Mentor muốn apply vào nền tảng thì phải điền thông tin
        // sau đó Admin sẽ duyệt 

// Mentor thì đi vào nền tảng, tạo những lịch rảnh và các hs sẽ book lịch rãnh đó 

// Để 1 User có thể trở thành 1 Mentor 
    // User sẽ điền thông tin để apply trở thành Mentor 
        // 1 User sẽ có yêu cầu (ApplyRequest) để trở thành Mentor
            // Khi mà người dùng đưa cho mình file Cv, chúng ta sẽ upload file đó lên Cloud 
        // => MQH 1 - N: Có thể có đơn bị từ chối hoặc được duyệt 

    // Khi mà có User apply nộp đơn, thì hệ thống thông báo cho Admin
    // Khi Admin duyệt (Từ chối, chấp nhận) thì phải thông báo cho user 
    // Admin sẽ duyệt thông tin đó, nếu thông tin hợp lệ thì sẽ duyệt 
    // Nếu được duyệt thì User đó sẽ trở thành Mentor
    // Chỉ có User nào có quyền Admin thì mới đc sử dụng API như lấy đơn hệ thống/phê duyệt đơn 

    // API:
        // Tạo đơn 
            //(Dành cho User)
            // POST /api/applyRequest
                // Để gọi được API này, cần CV và mô tả bản thân 

        // Lấy các đơn apply của tôi
            // GET /api/applyRequest/me
            // Mentor cần api này để kiểm tra xem tiến độ | tình trạng của đơn 

        // Lấy tất cả các đơn apply (Dành cho admin)
            // GET /api/applyRequest
            // Admin cần api này để duyệt đơn apply của người dùng 
        
        // Lấy thông tin chi tiết của đơn này
            // Dành cho Admin và User
            // GET /api/applyRequest/{id}

        // Duyệt đơn apply 
            // Dành cho Admin 
            // POST /api/applyRequest/{id}/review
            // Khi duyệt đơn thì Admin có thể chọn duyệt hay từ chối 
            // Nếu từ chối thì phải có lý do từ chối 
            // Khi mà duyệt đơn xong role của User phải được đổi thành Mentor
            // Khi duyệt đơn thì hệ thống phải thông báo cho User về kết quả của đơn apply đó 

// Kĩ thuật snapshot: 
    // Dùng để lưu dữ liệu ban đầu để dễ truy xuất 
    // Đầu tiên a có bản
    // Trong năm 2026, anh Tân bán 1 cái Áo với giá 1000
    // Sau do, Bao thay "Ao dep qua, muon mua cho crush"
    // Bao mua don hang (voi ID O1) tong la 3000,
    // trong don hang co 2 san pham, P1 va P2
    // Thời gian đưa trôi thấm thoá
    // Bây giờ là năm 2027, anh Tân đổi giá Áo (Id 1) thành 2000
    // Sau do bao chia tay ny, va bao doi lai qua, luc do
    // bao lay hoa don voi san pham P1,
    // Crush thi keu la ngay xua a mua em co 1000 ah
    // Sao gio anh Bao doi lai 2000
    

// App này gồm bao nhiu ngưười sài: 
    // Admin: 
        // Quản lí User (Hs và Mentor) của nền tảng 
        // Quản lí và phê duyệt đơn để trở thành Mentor

    // Mentor: 
        // Tạo lịch rãnh cho HS book
        // Quản lí lịch Book (Dời lịch, Hủy lịch) 
        // Quản lí Profile 

    // Học sinh: 
        // Tạo lịch rãnh cho HS Mentor
        // Quản lí lịch Book (Dời lịch, Hủy lịch) 
        // Quản lí Profile 

// 4W 1H 

// WHAT
        // Vấn đề | Sự Việc này là cái gì
// WHY
        // Tại sao lại có vấn đề này, tại sao lại cần giải quyết vấn đề này
// WHEN
        // Khi nào thì vấn đề này xảy ra, khi nào thì cần giải quyết vấn đề này
// WHERE
        // Ở đâu thì vấn đề này xảy ra, ở đâu thì cần giải quyết vấn đề này
// HOW
        // Giải quyết như thế nào











  
  
  
  
  
  
  
  
  
  
  