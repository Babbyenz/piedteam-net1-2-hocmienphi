namespace piedteam_net1_2_hocmienphi.service.Utils.Mail;

public class MailContent
{
    public required string To { get; set; } // Địa chỉ gửi đến 
    public string Subject { get; set; } // Chủ đề (tiêu đề email)
    public string Body { get; set; } // Nội dung (hỗ trợ HTML) của email 
}