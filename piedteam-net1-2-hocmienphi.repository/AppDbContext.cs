using Microsoft.EntityFrameworkCore;
using piedteam_net1_2_hocmienphi.repository.entity;
namespace piedteam_net1_2_hocmienphi.repository;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DbSet<Mentor> Mentors { get; set; }
    
    public DbSet<MentorCategory> MentorCategories { get; set; }
    
    public DbSet<Booking> Bookings { get; set; }
    
    public DbSet<MentorFreeTime> MentorFreeTimes { get; set; }
    
    public DbSet<ApplyRequest> ApplyRequests { get; set; }
    
    public DbSet<ApplyRequestCategory> ApplyRequestCategories { get; set; }
    
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) //Nhận cấu hình DB từ Program.cs
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder) //Cấu hình chi tiêt 
    {
        modelBuilder.Entity<User>(builder =>
        {
            builder.Property(u => u.Email)
                .IsRequired() //bắt buộc có dữ liệu 
                .HasMaxLength(255); //tối đa 255 kí tự 
            
            // kĩ thuật Indexing là gì (tối ưu tốc độ DB) 
            builder
                .HasIndex(u => u.Email) //Indexing là mục lục để dễ tìm hơn 
                
                .IsUnique(); //email k đc trùng
            
            builder
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);


            builder
                .HasOne(u => u.Mentor)
                .WithOne(m => m.User)
                .HasForeignKey<Mentor>(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.ApplyRequests) 
                .WithOne(m => m.User) // 1meeting thuộc về 1user
                .HasForeignKey(m => m.UserId) //meeting có khóa ngoại là UserId
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Bookings) // 1user có nhìu meeting
                .WithOne(x => x.User) // 1meeting thuộc về 1user
                .HasForeignKey(x => x.UserId) //meeting có khóa ngoại là UserId
                .OnDelete(DeleteBehavior.Cascade);

            //Khi mình chỉ định việc khai báo các mối quan hệ ở đây, 
            // các mqh sẽ k bao giờ bị tạo thiếu 

            // Mình có thể config các behavior (hành động)
            // khi có sự thay đổi dữ liệu giữa các mqh

            // Ví dụ: 1 User có nhiều Meetings, vậy User đó bị xóa đi 
            // Thì các Meetings sẽ xử lí ntn - nó sống chết ra sao 

            // => OnDelete sẽ làm sáng tỏ 
            
            // DeleteBehavior.Cascade: khi 1 User bị xóa, thì Meetings liên quan cx bị xóa theo.
            // DeleteBehavior.Restrict: Ngăn chặn việc xóa 1 User nếu có Meetings liên quan tồn tại.
                //(Tham chiếu tới PK tồn tại) 
                // 1 Meeting còn thuộc về 1 User thì k xóa đc 
                
            // DeleteBehavior.NoAction: Không thực hiện hành động gì đặc biệt khi User bị xóa.
                // (Gần giống Restrict, xử lí ở DB)
            // DeleteBehavior.setNull: Khi 1 User bị xóa, thì trường UserId trong bảng Meeting sẽ đc đặt thành Null.
                // (Áp dụng khi trường FK cho phép NULL)
                
            // Seed Data
            
            /*
            * // List<User> list = new List<User>()
              // {
              //     new User()
              //     {
              //         Id = Guid.NewGuid(),
              //         FirstName = "Quan",
              //         LastName = "Nguyen",
              //         Email = "quanq2903@gmail.com",
              //         Password = "112233",
              //         Role = "Student",
              //         Age = "18",
              //         IsDeleted = false,
              //     },
              //
              //     new User()
              //     {
              //         Id = Guid.NewGuid(),
              //         FirstName = "Quan",
              //         LastName = "Nguyen",
              //         Email = "quanq2903@gmail.com",
              //         Password = "112233",
              //         Role = "Student",
              //         Age = "18",
              //         IsDeleted = false,
              //     }
              // };
            */

            List<User> list = new List<User>()
            {

            };
            for (int i = 0; i <= 1000; i++)
            {
                var newUser = new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "FirstName" + i,
                    LastName = "LastName" + i,
                    Email = "email" + i + "@email.com",
                    Password = "112233",
                    Role = "Student",
                    Age = "18",
                    IsDeleted = false,
                    Nickname = "Nickname" + i,
                };
                list.Add(newUser);
            }
            builder.HasData(list);
            

        });

        modelBuilder.Entity<Category>(builder =>
        {
            builder
                .HasMany(u => u.MentorCategories)
                .WithOne(m => m.Category)
                .HasForeignKey(u => u.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.ApplyRequestCategories) 
                .WithOne(m => m.Category) 
                .HasForeignKey(m => m.CategoryId) 
                .OnDelete(DeleteBehavior.Cascade
                );
            
        });
        
        modelBuilder.Entity<ApplyRequest>(builder =>
        {
            builder
                .HasMany(u => u.ApplyRequestCategories) 
                .WithOne(m => m.ApplyRequest) 
                .HasForeignKey(m => m.ApplyRequestId) 
                .OnDelete(DeleteBehavior.Cascade
                );
            
        });
        
        modelBuilder.Entity<Mentor>(builder =>
        {
            builder
                .HasMany(u => u.MentorCategories) 
                .WithOne(m => m.Mentor)
                .HasForeignKey(m => m.MentorId) 
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.Bookings) 
                .WithOne(m => m.Mentor)
                .HasForeignKey(m => m.MentorId) 
                .OnDelete(DeleteBehavior.Cascade);
            
            builder
                .HasMany(u => u.MentorFreeTimes) 
                .WithOne(m => m.Mentor)
                .HasForeignKey(m => m.MentorId) 
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        

        
    }
}

// TechLead kêu: Eh ku tạo mới table đi 
// Em hiểu ngay mô hình 3 lớp (Trong file program): đi vào Repo 
// Em tìm ngay Folder Entity: Tạo mới Class Category
// Em khai báo các field: Id, Name,...
// Em khai báo các field liên quan tới mối quan hệ: ParentId, Parent, Children 
// Em đi vào DBContext khai báo 
// Em chạy Migration đẻ tạo ra file migration (Các phiên bản của DB)
// Em Update database 

// Các bước tạo lại migration:
    // Drop database 
    // Remove last migration
    // create migtation 
    // update db 
    