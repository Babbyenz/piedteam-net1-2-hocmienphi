namespace piedteam_net1_2_hocmienphi.repository;

    public class Class1
    {
    }
    // CODE FIRST | DATABASE FIRST 
    // ORM

    // Thông thường để code có thể làm việc được với Database, 
        // Thì mình cần phải ánh xạ (mapping) từ các table lên code để dễ dàng thao tác với dữ liệu ,
        
    // Database First: 
    // Mình làm việc với 1 Database đã có sẵn. (Create Database bằng SQL) 
    // Vô tạo Database nè, setup các field, các mối quan hệ, ràng buộc trong DB
    // Sau đó ở trên code sử dụng các Driver hoặc thư viện ORM để kết nối xuống Database
    // Ở trên code sẽ tạo các class tương ứng với các table trong Database 
    // Thằng này sử dụng khi nào: Khi Database đã có sẵn và đang được sử dụng trong
        // nhiều năm. Được join vào dự án Maintain

    // Code First: 
    // mình sẽ không setup Database thủ công bằng các câu lệnh Query
        // Create Database, Create Contraint
    // Mình sẽ Design Database bằng các class trên Code, trên code setup như thế nào 
        // thì Database sẽ được tạo ra như thế đó 
        // Mình setup trên code các field, các relationship 
        // Sau đó mình ánh xạ các đoạn code đó để tạo ra các table trong Database 
    
    // Vậy thì làm thế nào để ánh xạ được từ Code xuống các table trong Database 
        // Câu trả lời: ORM - Object Relational Mapping 
        // Nó sẽ là thằng trung gian đứng giữa làm nhiều thứ 
            // Nó sẽ đọc các class trên code, đọc các attribute, đọc các cấu hình 
                // sau đó tạo ra các câu lệnh SQL để tạo các bảng.

            // Nó cũng là thằng kết hợp với LINQ
                // Khi sử dụng các hàm Where..., Translate sang SQL
                // .WHERE() => Select * from table where...

    // Thằng này được sử dụng khi: 
        // Mình mới bắt đầu dự án, chưa có Database nào cả
        // Thiết kế bằng Code thì nó sẽ dễ dàng Maintain (dễ nhìn, dễ sửa) hơn so với các câu lệnh SQL 

    // Nếu mà k biết về LINQ + EF thì coi như mất 95% power


        
                
                
                
                
                
                
                
                
                
                
                