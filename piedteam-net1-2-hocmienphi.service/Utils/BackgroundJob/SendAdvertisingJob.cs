namespace piedteam_net1_2_hocmienphi.service.Utils.BackgroundJob;

public class SendAdvertisingJob
{
    
    // Background Job:
        // là 1 tác vụ được thực thi ngầm,
        // tách biệt khỏi luồng xử lý chính 
        // Tác vụ này do hệ thống thực thi
        
        // Khi nào server mình sẽ chạy công việc này:
            // Setup định kỳ (CronJob) 
                // + Cứ mỗi 7h sáng là gửi thư quảng cáo cho khách hàng
                // + Xóa mềm (IsDeleted = true), theo thời gian
                // những record xóa mềm sẽ tăng lên,
                // nhưng mà những dòng record này là những dòng chúng ta k động đến 
                // => Dữ liệu dư thừa, database nặng lên, định kì phải xóa nó đi, 1 tuần xóa 1 lần. 
                
            // Là mình sẽ nhờ nó thay mình làm công việc này giúp mình (Bận quá) - (Hơi nâng cao) 
                // Anh có 1 hàm là Register
                    // Xuống DB kiểm tra Email tồn tại (10ms)
                    // Tính toán để tạo Mật khẩu (Hashing - mã hóa) (20ms)
                    // Lưu User này vào Database (5ms)
                    // Gửi email thông báo verify tài khoản. (10s)
               // Logic gửi Email là 1 Logic rất là nặng 
                    // và tốn nhiều thời gian để hoàn thành 
                    // mình có thể chọn khi User bấm Register khỏi cho đợi email, trả ra response luôn 
                    // cái việc mà gửi email thông báo sẽ do luồng Background Job gửi 
                
      // Tôi muốn gửi mail cho tất cả các User trong hệ thống vòa lúc 7g sáng. 
        // Đối với User ít, tầm 20 thằng thì ngon ơ rồi. 
            // Cứ 7g sáng mình gọi 20 thằng đó ra gửi mail thôi 
        // Đối với 1 triệu User thì sao, 1 triệu thằng * 20s = 231 ngày 
        // Xài Task.WhenAll để tối ưu
            // (Trong vòng 20s đó, sẽ có 1000 thằng được gửi mail) 
            // (Vậy thì trong vòng 5g, sẽ gửi xong mail cho 1 triệu thằng) 
        // Anh ơi, tại sao trong 20s mình gửi 1 triệu thằng luôn đi ? 
        // Một Task được tạo ra, giống như mở thêm 1 luồng bất đồng bộ chạy 
        // Server xử lí 1 triuệ luồng cùng 1 lúc không nổi ! 
        // Nên là mình mới đánh đổi thời gian lại, dài hơn 1 xíu, 1 lần gửi 1000 thằng thôi 
        // Nhưng Server vẫn chịu được và sống tốt 
        
        // Thêm 1 problem nữa, bây giờ mình chỉ gửi mail 1 lúc 1000 thằng thôi,
            // Vậy thì chúng ta phải có 1 cái gì đó để dánh dấu 
            // là User nào đã gửi, user nào chưa gửi 
            // À, tôi nghĩ ngay isSend, ngon lành luôn, siêu System Design. 
            // Cái IsSend này chỉ ngon lành khi dòng dữ liệu này đụng đến 1 lần rồi thôi 
            // Ví dụ trạng thái của đơn hàng, isPending, nếu trong 1g không thanh toán 
            // thì mình chuyển thành isFail rồi thôi, k đụng tới nữa 
            
            // Vậy thì IsSend thì sao, nếu để IsSend ở User, mình sẽ k biết được 
            // là ngày nào đã gửi hay chưa, ví dụ: Ngày 6 để IsSend = true, qua ngày 7 IsSend = true luôn,
            // vậy hệ thống biết kiểu gì gửi 
        
        
            
}